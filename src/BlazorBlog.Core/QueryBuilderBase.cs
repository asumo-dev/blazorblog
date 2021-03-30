using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Web;

namespace BlazorBlog.Core
{
    public abstract class QueryBuilderBase<TQueryBuilder, TEntity>
        where TQueryBuilder : QueryBuilderBase<TQueryBuilder, TEntity>
    {
        protected readonly NameValueCollection Params = new();

        public abstract string Build();

        protected string GetQueryString()
        {
            var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);

            foreach (string key in Params)
            {
                httpValueCollection[key] = Params[key];
            }

            // ToString of NameValueCollection that is returned by HttpUtility.ParseQueryString generates a query string.
            var s = httpValueCollection.ToString();

            if (s == null)
            {
                return string.Empty;
            }

            return '?' + s;
        }

        protected IList<string> GetPropertyNames()
        {
            var members = typeof(TEntity).GetMembers();
            var names = new List<string>();

            foreach (var info in members)
            {
                var propertyName = GetPropertyName(info);
                if (propertyName != null)
                {
                    names.Add(propertyName);
                }
            }

            return names;
        }

        protected TQueryBuilder AddParam(string key, string value)
        {
            if (Params.Get(key) == null)
            {
                Params.Add(key, value);
            }

            return (TQueryBuilder)this;
        }

        protected string GetPropertyName<TKey>(Expression<Func<TEntity, TKey>> selector)
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

        protected string? GetPropertyName(MemberInfo info)
        {
            if (info.MemberType != MemberTypes.Property)
            {
                return null;
            }

            var jsonName = info.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;

            return jsonName ?? info.Name;
        }
    }
}
