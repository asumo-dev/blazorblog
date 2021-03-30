using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsBlogRepository : IBlogRepository
    {
        private readonly IMicroCmsClient _client;

        public MicroCmsBlogRepository(IMicroCmsClient client)
        {
            _client = client;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var builder = new MicroCmsQueryBuilder<BlogPostEntity>()
                .Limit(postsPerPage)
                .Offset(postsPerPage * page)
                .OrderByDescending(m => m.PublishedAt);
            var entries = await _client.GetContentsAsync(builder);

            if (entries == null)
            {
                return PagedPostCollection.Empty(postsPerPage);
            }
            
            return new PagedPostCollection
            {
                Posts = entries.Contents.Select(m => m.ToBlogPost()).ToList().AsReadOnly(),
                CurrentPage = page,
                TotalPosts = entries.TotalCount,
                PostsPerPage = postsPerPage
            };
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            var builder = new MicroCmsQueryBuilder<BlogPostEntity>()
                .ContentIdIs(slug);
            var entity = await _client.GetContentAsync(builder);

            return entity?.ToBlogPost();
        }
    }
}
