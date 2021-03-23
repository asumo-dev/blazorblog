using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Models;
using BlazorBlog.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorBlog.Strapi
{
    public class StrapiBlogRepository : IBlogRepository
    {
        private readonly ILogger<StrapiBlogRepository> _logger;
        private readonly StrapiClient _strapiClient;
        private readonly string _baseUrl;

        public StrapiBlogRepository(
            IOptions<StrapiOptions> options,
            IHttpClientFactory httpClientFactory,
            ILogger<StrapiBlogRepository> logger)
        {
            options.Value.ThrowsIfInvalid();
            
            _logger = logger;
            _baseUrl = options.Value.BaseUrl;

            _strapiClient = new StrapiClient(options.Value.BaseEndpoint, httpClientFactory.CreateClient());
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            IList<PostContent>? postContents;
            var count = 0;
            
            try
            {
                postContents = await _strapiClient.GetAsync(@params: new NameValueCollection
                {
                    {"_start", (postsPerPage * page).ToString()},
                    {"_limit", postsPerPage.ToString()},
                    { "_sort", "published_at:DESC"}
                });

                count = await _strapiClient.CountAsync();
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
                postContents = await _strapiClient.GetAsync(@params: new NameValueCollection
                {
                    {"slug_eq", slug},
                    {"_limit", "1"}
                });
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
