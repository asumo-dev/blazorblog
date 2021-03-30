using System;
using System.Linq.Expressions;
using BlazorBlog.Core;

namespace BlazorBlog.Strapi
{
    public class StrapiQueryBuilder<TEntity> : QueryBuilderBase<StrapiQueryBuilder<TEntity>, TEntity>
    {
        public StrapiQueryBuilder<TEntity> Start(int start)
            => AddParam("_start", start.ToString());

        public StrapiQueryBuilder<TEntity> Limit(int limit)
            => AddParam("_limit", limit.ToString());

        public StrapiQueryBuilder<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
            => AddParam("_sort", $"{GetPropertyName(keySelector)}:DESC");

        public StrapiQueryBuilder<TEntity> Order<TKey>(Expression<Func<TEntity, TKey>> keySelector)
            => AddParam("_sort", $"{GetPropertyName(keySelector)}:ASC");

        public StrapiQueryBuilder<TEntity> Eq<TKey>(Expression<Func<TEntity, TKey>> keySelector, string value)
            => AddParam($"{GetPropertyName(keySelector)}_eq", value);

        public override string Build()
            => GetQueryString();
    }
}
