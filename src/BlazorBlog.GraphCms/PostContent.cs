using System;
using System.Text.Json.Serialization;
using BlazorBlog.Core.Models;

namespace BlazorBlog.GraphCms
{
    public record PostContent
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
            
        [JsonPropertyName("slug")]
        public string Slug { get; set; }
            
        [JsonPropertyName("content")]
        public ContentContent Content { get; set; }
            
        [JsonPropertyName("publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        public record ContentContent
        {
            [JsonPropertyName("html")]
            public string Html { get; set; }
        }

        public BlogPost ToBlogPost()
        {
            return new(Title, Slug, Content.Html, PublishedAt.LocalDateTime);
        }
    }
}
