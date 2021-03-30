using System;
using System.Text.Json.Serialization;
using BlazorBlog.Core.Models;

namespace BlazorBlog.Ghost
{
    public record PostContent
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("html")]
        public string? Html { get; set; }

        [JsonPropertyName("published_at")]
        public DateTime? PublishedAt { get; set; }

        public BlogPost ToBlogPost()
        {
            if (Title == null || Slug == null || Html == null || PublishedAt == null)
                throw new InvalidOperationException(
                    "Content fields cannot be null. Check your settings on Ghost website.");

            return new BlogPost(Title, Slug, Html, PublishedAt.Value);
        }
    }
}
