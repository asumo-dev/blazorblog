using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BlazorBlog.MicroCms
{
    public interface IMicroCmsClient
    {
        Task<T?> GetAsync<T>(NameValueCollection? queryParams);
        
        Task<T?> GetAsync<T>(string id, NameValueCollection? queryParams);
    }
}
