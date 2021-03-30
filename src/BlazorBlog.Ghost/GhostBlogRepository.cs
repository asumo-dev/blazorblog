using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.Logging;

namespace BlazorBlog.Ghost
{
    public class GhostBlogRepository  : IBlogRepository
    {
        private readonly ILogger<GhostBlogRepository> _logger;
        private readonly IGhostClient _client;

        public GhostBlogRepository(IGhostClient client, ILogger<GhostBlogRepository> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            PostsResponse<PostContent>? postsResponses;
            
            try
            {
                var queryBuilder = new GhostQueryBuilder<PostContent>()
                    .Limit(postsPerPage)
                    .Page(page + 1)
                    .OrderByDescending(m => m.PublishedAt);
                postsResponses = await _client.GetPostsAsync(queryBuilder);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from Ghost");
                postsResponses = null;
            }

            if (postsResponses?.Posts == null)
            {
                return PagedPostCollection.Empty(postsPerPage);
            }

            return Utils.ToPagedPostCollection(postsResponses);
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            PostsResponse<PostContent>? postsResponses;
            
            try
            {
                var queryBuilder = new GhostQueryBuilder<PostContent>()
                    .Slug(slug);
                postsResponses = await _client.GetPostsAsync(queryBuilder);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from Ghost");
                postsResponses = null;
            }

            return postsResponses?.Posts?.FirstOrDefault()?.ToBlogPost();
        }
    }
}
