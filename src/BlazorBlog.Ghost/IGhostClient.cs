using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BlazorBlog.Ghost
{
    public interface IGhostClient
    {
        Task<PostsResponse?> GetPostsAsync(string? slug = null, NameValueCollection? @params = null);
    }
}
