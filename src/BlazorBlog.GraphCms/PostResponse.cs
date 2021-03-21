using System.Text.Json.Serialization;

namespace BlazorBlog.GraphCms
{
    public record PostResponse
    {
        [JsonPropertyName("post")]
        public PostContent Post { get; set; }
    }
}
