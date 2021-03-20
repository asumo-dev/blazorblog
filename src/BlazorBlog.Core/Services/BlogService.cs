using System.Threading.Tasks;
using BlazorBlog.Models;

namespace BlazorBlog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repository;

        public BlogService(IBlogRepository repository)
        {
            _repository = repository;
        }

        public Task<PagedPostCollection> GetPagedPostsAsync(int page = 0, int postsPerPage = 5)
        {
            return _repository.GetPagedPostsAsync(page, postsPerPage);
        }

        public Task<BlogPost?> GetPostAsync(string id)
        {
            return _repository.GetPostAsync(id);
        }
    }
}