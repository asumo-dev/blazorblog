using System;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using Contentful.Core.Models;

namespace BlazorBlog.Contentful
{
    public class BlogPostEntry
    {
        public SystemProperties? Sys { get; set; }
        
        public string? Title { get; set; }
        
        public string? Slug { get; set; }
        
        public Document? Body { get; set; }

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