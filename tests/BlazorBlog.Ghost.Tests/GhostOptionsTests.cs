using System;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class GhostOptionsTests
    {
        [Theory]
        [InlineData("api_url", "content_api_key", false)]
        [InlineData("api_url", null, true)]
        [InlineData(null, "content_api_key", true)]
        public void ThrowsIfInvalid_Works(string? apiUrl, string? contentApiKey, bool isExceptionExpected)
        {
            var ghostOptions = new GhostOptions
            {
                ApiUrl = apiUrl,
                ContentApiKey = contentApiKey
            };

            if (isExceptionExpected)
            {
                Assert.Throws<InvalidOperationException>(() =>
                    ghostOptions.ThrowsIfInvalid());
            }
        }
    }
}
