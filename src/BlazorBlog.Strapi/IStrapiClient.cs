using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BlazorBlog.Strapi
{
    public interface IStrapiClient
    {
        Task<IList<PostContent>?> GetAsync(string? id = null, NameValueCollection? @params = null);
        Task<int> CountAsync();
    }
}
