using System;
using System.Collections.Specialized;
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
            PostsResponse? postsResponses;
            
            try
            {
                var @params = CreatePagedPostsParams(page, postsPerPage);
                postsResponses = await _client.GetPostsAsync(@params: @params);
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

            return postsResponses.ToPagedPostCollection();
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            PostsResponse? postsResponses;
            
            try
            {
                var @params = CreatePostParams();
                postsResponses = await _client.GetPostsAsync(slug, @params);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from Ghost");
                postsResponses = null;
            }

            return postsResponses?.Posts?.FirstOrDefault()?.ToBlogPost();
        }

        private static NameValueCollection CreatePagedPostsParams(int page, int postsPerPage)
        {
            return new(CreatePostParams())
            {
                {"limit", postsPerPage.ToString()},
                {"page", (page + 1).ToString()},
                {"order", "published_at DESC"}
            };
        }

        private static NameValueCollection CreatePostParams()
        {
            return new()
            {
                {"fields", "title,slug,html,published_at"}
            };
        }
    }
}
