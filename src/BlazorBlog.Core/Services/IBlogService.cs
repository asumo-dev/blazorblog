using System.Threading.Tasks;
using BlazorBlog.Core.Models;

namespace BlazorBlog.Core.Services
{
    public interface IBlogService
    {
        Task<PagedPostCollection> GetPagedPostsAsync(int page = 0, int postsPerPage = 5);

        Task<BlogPost?> GetPostAsync(string id);
    }
}