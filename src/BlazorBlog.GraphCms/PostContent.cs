using System;
using System.Text.Json.Serialization;
using BlazorBlog.Core.Models;

namespace BlazorBlog.GraphCms
{
    public record PostContent
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
            
        [JsonPropertyName("slug")]
        public string? Slug { get; set; }
            
        [JsonPropertyName("content")]
        public ContentContent? Content { get; set; }
            
        [JsonPropertyName("publishedAt")]
        public DateTimeOffset? PublishedAt { get; set; }

        public record ContentContent
        {
            [JsonPropertyName("html")]
            public string? Html { get; set; }
        }

        public BlogPost ToBlogPost()
        {
            if (Title == null || Slug == null || Content?.Html == null || PublishedAt == null)
                throw new InvalidOperationException(
                    "Content fields cannot be null. Check your settings on Ghost website.");

            return new(Title, Slug, Content.Html, PublishedAt.Value.LocalDateTime);
        }
    }
}
