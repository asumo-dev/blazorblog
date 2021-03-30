using System;
using System.Text.Json.Serialization;
using BlazorBlog.Core.Models;

namespace BlazorBlog.MicroCms
{
    public class BlogPostEntity
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        
        [JsonPropertyName("body")]
        public string? BodyHtml { get; set; }

        [JsonPropertyName("publishedAt")]
        public DateTime? PublishedAt { get; set; }
        
        public BlogPost ToBlogPost()
        {
            if (Id == null || Title == null || BodyHtml == null || PublishedAt == null)
                throw new InvalidOperationException(
                    "Entity fields cannot be null. Check your settings on microCMS website.");
            
            return new BlogPost(Title, Id, BodyHtml, PublishedAt.Value);
        }
    }
}
