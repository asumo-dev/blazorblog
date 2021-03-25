using System;
using System.Diagnostics.CodeAnalysis;

namespace BlazorBlog.Ghost
{
    public class GhostOptions
    {
        public string? ApiUrl { get; set; }
        
        public string? ContentApiKey { get; set; }

        [MemberNotNull(nameof(ApiUrl))]
        [MemberNotNull(nameof(ContentApiKey))]
        public void ThrowsIfInvalid()
        {
            if (ApiUrl == null || ContentApiKey == null)
            {
                throw new InvalidOperationException("GhostOptions is invalid.");
            }
        }
    }
}
