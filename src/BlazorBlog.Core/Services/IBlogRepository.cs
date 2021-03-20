using System.Threading.Tasks;
using BlazorBlog.Models;

namespace BlazorBlog.Services
{
    public interface IBlogRepository
    {
        Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage);
        
        Task<BlogPost?> GetPostAsync(string slug);
    }
}