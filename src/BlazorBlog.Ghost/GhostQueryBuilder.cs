using System;
using System.Linq.Expressions;
using BlazorBlog.Core;

namespace BlazorBlog.Ghost
{
    public class GhostQueryBuilder<T> : QueryBuilderBase<GhostQueryBuilder<T>, T>
    {
        private string _slug = string.Empty;

        public GhostQueryBuilder()
        {
            var propertyNames = GetPropertyNames();
            var str = string.Join(',', propertyNames);

            if (string.IsNullOrEmpty(str))
            {
                throw new InvalidOperationException("The specified class doesn't have any properties.");
            }

            AddParam("fields", str);
        }

        public GhostQueryBuilder<T> Limit(int limit)
            => AddParam("limit", limit.ToString());

        public GhostQueryBuilder<T> Page(int page)
            => AddParam("page", page.ToString());

        public GhostQueryBuilder<T> Slug(string slug)
        {
            _slug = slug;
            return this;
        }

        public GhostQueryBuilder<T> Order<TKey>(Expression<Func<T, TKey>> keySelector)
            => AddParam("order", GetPropertyName(keySelector));

        public GhostQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
            => AddParam("order", $"{GetPropertyName(keySelector)} DESC");

        public GhostQueryBuilder<T> ApiKey(string key)
            => AddParam("key", key);

        public override string Build() =>
            string.IsNullOrEmpty(_slug)
                ? GetQueryString()
                : $"slug/{_slug}{GetQueryString()}";
    }
}
