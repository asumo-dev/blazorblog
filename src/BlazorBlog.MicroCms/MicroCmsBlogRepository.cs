using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsBlogRepository : IBlogRepository
    {
        private readonly MicroCmsClient _client;

        public MicroCmsBlogRepository(IOptions<MicroCmsOptions> options, IHttpClientFactory factory)
        {
            _client = new MicroCmsClient(
                options.Value.Endpoint,
                options.Value.ApiKey,
                factory.CreateClient());
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

            return entity.ToBlogPost();
        }
    }
}