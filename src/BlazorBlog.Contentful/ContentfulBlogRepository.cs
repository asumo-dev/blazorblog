using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using Contentful.Core;
using Contentful.Core.Search;

namespace BlazorBlog.Contentful
{
    public class ContentfulBlogRepository : IBlogRepository
    {
        private readonly IContentfulClient _client;

        public ContentfulBlogRepository(IContentfulClient contentfulClient)
        {
            _client = contentfulClient;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var builder = new QueryBuilder<BlogPostEntry>()
                .ContentTypeIs("blogPost")
                .Skip(postsPerPage * page)
                .Limit(postsPerPage);
            var entries = await _client.GetEntries(builder);
            
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
            var entry = await _client.GetEntries(builder);
            
            var post = entry.Items.FirstOrDefault();
            if (post == null) return null;
            
            return await post.ToBlogPostAsync();
        }
    }
}
