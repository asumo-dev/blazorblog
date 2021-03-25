using System;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using Contentful.Core.Models;

namespace BlazorBlog.Contentful
{
    public class BlogPostEntry
    {
        public SystemProperties? Sys { get; set; } = default;

        public string? Title { get; set; } = default;

        public string? Slug { get; set; } = default;

        public Document? Body { get; set; } = default;

        public async Task<BlogPost> ToBlogPostAsync()
        {
            if (Sys == null || Title == null || Slug == null || Body == null)
                throw new InvalidOperationException(
                    "Entity fields cannot be null. Check your settings on Contentful website.");
            
            var htmlRenderer = new HtmlRenderer();
            var html = await htmlRenderer.ToHtml(Body);
            return new BlogPost(Title, Slug, html, Sys.CreatedAt ?? default);
        }
    }
}
