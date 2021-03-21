using System.Text.Json.Serialization;

namespace BlazorBlog.GraphCms
{
    public record PagedPostsResponse
    {
        [JsonPropertyName("postsConnection")]
        public PostsConnectionContent PostsConnection { get; set; }
        
        public record PostsConnectionContent
        {
            [JsonPropertyName("aggregate")]
            public AggregateContent Aggregate { get; set; }

            [JsonPropertyName("edges")]
            public EdgeContent[] Edges { get; set; }
            
            [JsonPropertyName("pageInfo")]
            public PageInfoContent PageInfo { get; set; }
            
            public record AggregateContent
            {
                [JsonPropertyName("count")]
                public int Count { get; set; }
            }

            public record EdgeContent
            {
                [JsonPropertyName("node")]
                public PostContent Node { get; set; }
            }

            public record PageInfoContent
            {
                [JsonPropertyName("pageSize")]
                public int PageSize { get; set; }
            }
        }
    }
}
