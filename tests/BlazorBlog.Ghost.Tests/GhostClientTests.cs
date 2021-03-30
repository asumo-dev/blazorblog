using System;
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
        private const string PostsEndpoint = "https://example.com/ghost/api/v3/content/posts/";

        [Fact]
        public async Task GetPostsAsync_WithSlug_RequestsToCorrectEndpoint()
        {
            var mockHttp = CreateMockHttp($"{PostsEndpoint}slug/welcome?fields=title%2cslug%2chtml%2cpublished_at&key=content_api_key", "{}");
            var ghostClient = new GhostClient(mockHttp.ToHttpClient(), ApiUrl, ContentApiKey);
            var queryBuilder = new GhostQueryBuilder<PostContent>()
                .Slug("welcome");

            await ghostClient.GetPostsAsync(queryBuilder);

            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Fact]
        public async Task GetPostsAsync_WithParams_RequestsToCorrectEndpoint()
        {
            var mockHttp = CreateMockHttp(
                $"{PostsEndpoint}?fields=title%2cslug%2chtml%2cpublished_at&page=1&limit=5&key=content_api_key",
                "{}");
            var ghostClient = new GhostClient(mockHttp.ToHttpClient(), ApiUrl, ContentApiKey);
            var queryBuilder = new GhostQueryBuilder<PostContent>()
                .Page(1)
                .Limit(5);

            await ghostClient.GetPostsAsync(queryBuilder);

            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Fact]
        public async Task GetPostsAsync_ReturnsCorrectly()
        {
            var actualGhostResponse = TestData.GhostResponseJson;
            var mockHttp = CreateMockHttp(
                $"{PostsEndpoint}?fields=title%2cslug%2chtml%2cpublished_at&key=content_api_key", actualGhostResponse);

            var ghostClient = new GhostClient(mockHttp.ToHttpClient(), ApiUrl, ContentApiKey);
            var queryBuilder = new GhostQueryBuilder<PostContent>();

            // Act
            var actual = await ghostClient.GetPostsAsync(queryBuilder);

            var expected = TestData.PostsResponse;

            Assert.Equal(expected.Posts, actual?.Posts);
            Assert.Equal(expected.Meta, actual?.Meta);
        }

        [Fact]
        public async Task GetPostsAsync_ThrowsException_WhenApiReturnsInvalidData()
        {
            var invalidResponse = "123";
            var mockHttp = CreateMockHttp($"{PostsEndpoint}?fields=title%2cslug%2chtml%2cpublished_at&key=content_api_key", invalidResponse);
            var ghostClient = new GhostClient(mockHttp.ToHttpClient(), ApiUrl, ContentApiKey);
            var queryBuilder = new GhostQueryBuilder<PostContent>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await ghostClient.GetPostsAsync(queryBuilder);
            });

            mockHttp.VerifyNoOutstandingExpectation();
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
