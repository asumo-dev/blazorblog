using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class GhostClientTests
    {
        private const string ApiUrl = "https://example.com";
        private const string ContentApiKey = "content_api_key";
            
        [Fact]
        public async Task GetPostsAsync_WithSlug_RequestsToCorrectEndpoint()
        {
            var mockHttp = CreateMockHttp("https://example.com/ghost/api/v3/content/posts/slug?key=content_api_key", "{}");
            var strapiClient = new GhostClient(ApiUrl, ContentApiKey, mockHttp.ToHttpClient());

            await strapiClient.GetPostsAsync("slug");

            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Fact]
        public async Task GetPostsAsync_WithParams_RequestsToCorrectEndpoint()
        {
            var mockHttp = CreateMockHttp(
                "https://example.com/ghost/api/v3/content/posts?page=1&limit=5&key=content_api_key",
                "{}");
            var strapiClient = new GhostClient(ApiUrl, ContentApiKey, mockHttp.ToHttpClient());

            await strapiClient.GetPostsAsync(@params: new NameValueCollection
            {
                {"page", "1"},
                {"limit", "5"}
            });

            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Fact]
        public async Task GetPostsAsync_ReturnsCorrectly()
        {
            var actualGhostResponse = TestData.GhostResponseJson;
            var mockHttp = CreateMockHttp("https://example.com/ghost/api/v3/content/posts", actualGhostResponse);
            
            var strapiClient = new GhostClient(ApiUrl, ContentApiKey, mockHttp.ToHttpClient());

            // Act
            var actual = await strapiClient.GetPostsAsync();

            var expected = TestData.PostsResponse;

            Assert.Equal(expected.Posts, actual?.Posts);
            Assert.Equal(expected.Meta, actual?.Meta);
        }

        [Fact]
        public async Task GetPostsAsync_ThrowsException_WhenApiReturnsInvalidData()
        {
            var invalidResponse = "123";
            var mockHttp = CreateMockHttp("https://example.com/ghost/api/v3/content/posts", invalidResponse);
            var strapiClient = new GhostClient(ApiUrl, ContentApiKey, mockHttp.ToHttpClient());

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await strapiClient.GetPostsAsync();
            });
        }
        
        private MockHttpMessageHandler CreateMockHttp(string expectedEndpoint, string responseContent)
        {
            var mockHttp = new MockHttpMessageHandler(); 
            mockHttp.Expect(
                    HttpMethod.Get,
                    expectedEndpoint)
                .Respond("application/json", responseContent);

            return mockHttp;
        }
    }
}