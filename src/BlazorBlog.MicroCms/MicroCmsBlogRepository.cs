using System.Collections.Specialized;
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
            var entries = await _client.GetAsync<MicroCmsCollection<BlogPostEntity>>(
                new NameValueCollection
                {
                    {"fields", "title,id,body,publishedAt"},
                    {"limit", postsPerPage.ToString()},
                    {"offset", (postsPerPage * page).ToString()},
                    {"orders", "-publishedAt"}
                });

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
            var entity = await _client.GetAsync<BlogPostEntity>(slug, new NameValueCollection
            {
                {"fields", "title,id,body,publishedAt"}
            });

            return entity?.ToBlogPost();
        }
    }
}
