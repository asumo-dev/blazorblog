using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorBlog.Strapi
{
    public class StrapiBlogRepository : IBlogRepository
    {
        private readonly ILogger<StrapiBlogRepository> _logger;
        private readonly IStrapiClient _client;
        private readonly string _baseUrl;

        public StrapiBlogRepository(
            IStrapiClient client,
            IOptions<StrapiOptions> options,
            ILogger<StrapiBlogRepository> logger)
        {
            options.Value.ThrowsIfInvalid();

            _logger = logger;
            _baseUrl = options.Value.BaseUrl;

            _client = client;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            IList<PostContent>? postContents;
            var count = 0;
            
            try
            {
                var queryBuilder = new StrapiQueryBuilder<PostContent>()
                    .Start(postsPerPage * page)
                    .Limit(postsPerPage)
                    .OrderByDescending(m => m.PublishedAt);
                postContents = await _client.GetAsync(queryBuilder);

                count = await _client.CountAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from Strapi");
                postContents = null;
            }

            if (postContents == null)
            {
                return PagedPostCollection.Empty(postsPerPage);
            }
            
            return new PagedPostCollection
            {
                Posts = postContents.Select(m => m.ToBlogPost(_baseUrl)).ToList().AsReadOnly(),
                CurrentPage = page,
                TotalPosts = count,
                PostsPerPage = postsPerPage
            };
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            IList<PostContent>? postContents;
            
            try
            {
                var queryBuilder = new StrapiQueryBuilder<PostContent>()
                    .Eq(m =>  m.Slug, slug)
                    .Limit(1);
                postContents = await _client.GetAsync(queryBuilder);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from Strapi");
                return null;
            }

            return postContents?.FirstOrDefault()?.ToBlogPost(_baseUrl);
        }
    }
}
