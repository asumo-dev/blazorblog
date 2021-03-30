using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsCollection<T>
    {
        [JsonPropertyName(("contents"))]
        public ICollection<T> Contents { get; set; } = default!;

        [JsonPropertyName(("totalCount"))]
        public int TotalCount { get; set; }
        
        [JsonPropertyName(("offset"))]
        public int Offset { get; set; }
        
        [JsonPropertyName(("limit"))]
        public int Limit { get; set; }
    }
}
