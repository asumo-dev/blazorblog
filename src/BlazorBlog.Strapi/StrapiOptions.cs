using System;
using System.Diagnostics.CodeAnalysis;

namespace BlazorBlog.Strapi
{
    public class StrapiOptions
    {
        public string? BaseUrl { get; set; }
        
        public string? ContentName { get; set; }

        public string BaseEndpoint => $"{BaseUrl?.TrimEnd('/')}/{ContentName}";

        [MemberNotNull(nameof(BaseUrl))]
        [MemberNotNull(nameof(ContentName))]
        public void ThrowsIfInvalid()
        {
            if (BaseUrl == null || ContentName == null)
            {
                throw new InvalidOperationException("StrapiOptions is invalid.");
            }
        }
    }
}
