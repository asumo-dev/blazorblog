using System.Threading.Tasks;
using BlazorBlog.Core.Models;

namespace BlazorBlog.Core.Services
{
    public interface IBlogRepository
    {
        Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage);
        
        Task<BlogPost?> GetPostAsync(string slug);
    }
}