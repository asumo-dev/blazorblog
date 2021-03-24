using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using BlazorBlog.Models;

namespace BlazorBlog.Ghost
{
    public class PostsResponse
    {
        [JsonPropertyName("posts")]
        public IList<PostContent>? Posts { get; set; }

        [JsonPropertyName("meta")]
        public MetaContent? Meta { get; set; }

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

        public record MetaContent
        {
            [JsonPropertyName("pagination")]
            public PaginationContent? Pagination { get; set; }

            [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
            public record PaginationContent
            {
                [JsonPropertyName("page")]
                public int? Page { get; set; }

                [JsonPropertyName("limit")]
                public int? Limit { get; set; }

                [JsonPropertyName("pages")]
                public int? Pages { get; set; }

                [JsonPropertyName("total")]
                public int? Total { get; set; }

                [JsonPropertyName("next")]
                public int? Next { get; set; }

                [JsonPropertyName("prev")]
                public int? Prev { get; set; }
            }
        }

        public PagedPostCollection ToPagedPostCollection()
        {
            if (Posts == null ||
                Meta?.Pagination?.Page == null ||
                Meta.Pagination.Limit == null ||
                Meta.Pagination.Total == null)
                throw new InvalidOperationException(
                    "PostsResponse with null Posts or Meta cannot be converted to BlogPost.");

            return new PagedPostCollection
            {
                Posts = Posts.Select(p => p.ToBlogPost()).ToList().AsReadOnly(),
                CurrentPage = Meta.Pagination.Page.Value - 1,
                TotalPosts = Meta.Pagination.Total.Value,
                PostsPerPage = Meta.Pagination.Limit.Value
            };
        }
    }
}
