using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorBlog.Strapi
{
    public interface IStrapiClient
    {
        Task<IList<T>?> GetAsync<T>(StrapiQueryBuilder<T> queryBuilder);

        Task<int> CountAsync();
    }
}
