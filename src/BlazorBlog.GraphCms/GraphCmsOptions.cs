using System;
using System.Diagnostics.CodeAnalysis;

namespace BlazorBlog.GraphCms
{
    public class GraphCmsOptions
    {
        public string? Endpoint { get; set; } = default!;

        public string? ApiToken { get; set; } = default;

        [MemberNotNull(nameof(Endpoint))]
        public void ThrowsIfInvalid()
        {
            if (Endpoint == null)
            {
                throw new InvalidOperationException($"{nameof(GraphCmsOptions)} is invalid.");
            }
        }
    }
}
