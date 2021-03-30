using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BlazorBlog.Ghost
{
    public class PostsResponse<T>
    {
        [JsonPropertyName("posts")]
        public IList<T>? Posts { get; set; }

        [JsonPropertyName("meta")]
        public MetaContent? Meta { get; set; }

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
    }
}
