using System;
using BlazorBlog.Models;

namespace BlazorBlog.Services
{
    public class UriGenerator : IUriGenerator
    {
        public string Post(BlogPost post) => Post(post.Slug);
        
        public string Post(string slug) => $"posts/{Uri.EscapeDataString(slug)}";
        
        public string Posts(int? page = null)
        {
            return page.HasValue ? $"page/{page}" : "";
        }
    }
}