using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.Logging;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsBlogRepository : IBlogRepository
    {
        private readonly IMicroCmsClient _client;
        private readonly ILogger<MicroCmsBlogRepository> _logger;

        public MicroCmsBlogRepository(IMicroCmsClient client, ILogger<MicroCmsBlogRepository> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var builder = new MicroCmsQueryBuilder<BlogPostEntity>()
                .Limit(postsPerPage)
                .Offset(postsPerPage * page)
                .OrderByDescending(m => m.PublishedAt);

            MicroCmsCollection<BlogPostEntity>? entries;

            try
            {
                entries = await _client.GetContentsAsync(builder);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from microCMS");
                return PagedPostCollection.Empty(postsPerPage);
            }

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

            try
            {
                var entity = await _client.GetContentAsync(builder);

                return entity?.ToBlogPost();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from microCMS");

                return null;
            }
        }
    }
}
