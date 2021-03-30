using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsQueryBuilder<T>
    {
        private readonly NameValueCollection _params = new();
        private string _id = string.Empty;

        public MicroCmsQueryBuilder()
        {
            var members = typeof(T).GetMembers();
            var sb = new StringBuilder();

            foreach (var info in members)
            {
                var propertyName = GetPropertyName(info);
                if (propertyName != null)
                {
                    sb.Append(propertyName).Append(',');
                }
            }

            if (sb.Length == 0)
            {
                throw new InvalidOperationException("The specified class doesn't have any properties.");
            }

            AddParam("fields", sb.ToString().Remove(sb.Length - 1));
        }

        public MicroCmsQueryBuilder<T> ContentIdIs(string id)
        {
            _id = id;
            return this;
        }

        public MicroCmsQueryBuilder<T> Limit(int limit)
            => AddParam("limit", limit.ToString());

        public MicroCmsQueryBuilder<T> Offset(int offset)
            => AddParam("offset", offset.ToString());

        public MicroCmsQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
            => AddParam("order", $"-{GetPropertyName(keySelector)}");

        public MicroCmsQueryBuilder<T> Order<TKey>(Expression<Func<T, TKey>> keySelector)
            => AddParam("order", GetPropertyName(keySelector));

        private MicroCmsQueryBuilder<T> AddParam(string key, string value)
        {
            if (_params.Get(key) == null)
            {
                _params.Add(key, value);
            }

            return this;
        }

        private string GetPropertyName<TKey>(Expression<Func<T, TKey>> selector)
        {
            if (!(selector.Body is MemberExpression body))
            {
                throw new ArgumentException();
            }

            var propertyName = GetPropertyName(body.Member);
            if (propertyName == null)
            {
                throw new InvalidOperationException("The specified key is not a property.");
            }

            return propertyName;
        }

        private string? GetPropertyName(MemberInfo info)
        {
            if (info.MemberType != MemberTypes.Property)
            {
                return null;
            }

            var jsonName = info.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;

            return jsonName ?? info.Name;
        }

        public string Build()
        {
            var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);

            foreach (string key in _params)
            {
                httpValueCollection[key] = _params[key];
            }

            // ToString of NameValueCollection that is returned by HttpUtility.ParseQueryString generates a query string.
            var s = httpValueCollection.ToString();

            if (s == null)
            {
                return _id;
            }

            return $"{_id}?{s}";
        }
    }
}
