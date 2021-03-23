using System;
using System.Text.Json.Serialization;
using BlazorBlog.Models;
using Markdig;

namespace BlazorBlog.Strapi
{
    public record PostContent
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("content")]
        public string? ContentMarkdown { get; set; }

        [JsonPropertyName("published_at")]
        public DateTime? PublishedAt { get; set; }

        public BlogPost ToBlogPost(string? imgBaseUrl = null)
        {
            var html = ConvertContentMarkdownToHtml(imgBaseUrl);
            
            if (Title == null || Slug == null || html == null || PublishedAt == null)
                throw new InvalidOperationException(
                    "Content fields cannot be null. Check your settings on Strapi website.");
            
            return new(Title, Slug, html, PublishedAt.Value);
        }

        private string? ConvertContentMarkdownToHtml(string? imgBaseUrl)
        {
            if (ContentMarkdown == null) return null;

            var pipelineBuilder = new MarkdownPipelineBuilder();

            if (imgBaseUrl != null)
            {
                pipelineBuilder.UseUrlRewriter(link =>
                    link.IsImage && (link.Url?.StartsWith('/') ?? false)
                        ? $"{imgBaseUrl.TrimEnd('/')}{link.Url}"
                        : link.Url);
            }

            return Markdown.ToHtml(ContentMarkdown, pipelineBuilder.Build());
        }
    }
}
