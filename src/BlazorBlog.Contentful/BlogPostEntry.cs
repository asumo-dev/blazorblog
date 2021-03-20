﻿using System.Threading.Tasks;
using BlazorBlog.Models;
using Contentful.Core.Models;

namespace BlazorBlog.Contentful
{
    public class BlogPostEntry
    {
        public SystemProperties Sys { get; set; }
        
        public string Title { get; set; }
        
        public string Slug { get; set; }
        
        public Document Body { get; set; }

        public async Task<BlogPost> ToBlogPostAsync()
        {
            var htmlRenderer = new HtmlRenderer();
            var html = await htmlRenderer.ToHtml(Body);
            return new BlogPost(Title, Slug, html, Sys.CreatedAt ?? default);
        }
    }
}