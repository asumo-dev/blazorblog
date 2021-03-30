using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using Contentful.Core;
using Contentful.Core.Errors;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;

namespace BlazorBlog.Contentful
{
    public class ContentfulBlogRepository : IBlogRepository
    {
        private readonly IContentfulClient _client;
        private readonly ILogger<ContentfulBlogRepository> _logger;

        public ContentfulBlogRepository(
            IContentfulClient contentfulClient, ILogger<ContentfulBlogRepository> logger)
        {
            _client = contentfulClient;
            _logger = logger;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var builder = new QueryBuilder<BlogPostEntry>()
                .ContentTypeIs("blogPost")
                .Skip(postsPerPage * page)
                .Limit(postsPerPage);

            ContentfulCollection<BlogPostEntry>? entries;

            try
            {
                entries = await _client.GetEntries(builder);
            }
            catch (ContentfulException e)
            {
                _logger.LogError(e, "Failed to fetch from Contentful");
                return PagedPostCollection.Empty(postsPerPage);
            }

            var list = new List<BlogPost>();
            foreach (var entry in entries.Items)
            {
                list.Add(await entry.ToBlogPostAsync());
            }

            return new PagedPostCollection
            {
                Posts = list.AsReadOnly(),
                CurrentPage = page,
                TotalPosts = entries.Total,
                PostsPerPage = postsPerPage
            };
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            var builder = new QueryBuilder<BlogPostEntry>()
                .ContentTypeIs("blogPost")
                .FieldMatches(m => m.Slug, slug)
                .Limit(1);

            ContentfulCollection<BlogPostEntry>? entries;

            try
            {
                entries = await _client.GetEntries(builder);
            }
            catch (ContentfulException e)
            {
                _logger.LogError(e, "Failed to fetch from Contentful");
                return null;
            }

            var post = entries.Items.FirstOrDefault();
            if (post == null) return null;
            
            return await post.ToBlogPostAsync();
        }
    }
}
