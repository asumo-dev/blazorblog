using System;
using System.Linq.Expressions;
using BlazorBlog.Core;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsQueryBuilder<T> : QueryBuilderBase<MicroCmsQueryBuilder<T>, T>
    {
        private string _id = string.Empty;

        public MicroCmsQueryBuilder()
        {
            var propertyNames = GetPropertyNames();
            var str = string.Join(',', propertyNames);

            if (string.IsNullOrEmpty(str))
            {
                throw new InvalidOperationException("The specified class doesn't have any properties.");
            }

            AddParam("fields", str);
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

        public override string Build() => _id + GetQueryString();
    }
}
