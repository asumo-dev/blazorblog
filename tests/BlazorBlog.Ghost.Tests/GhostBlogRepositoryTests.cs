using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class GhostBlogRepositoryTests
    {
        private const string ApiUrl = "https://example.com";
        private const string ContentApiKey = "content_api_key";
        private const string BaseEndpoint = ApiUrl + "/ghost/api/v3/content/posts";
        
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var mockHttp = CreateMockHttp(
                $"{BaseEndpoint}?fields=title,slug,html,published_at&limit=5&page=3&order=published_at%20DESC&key={ContentApiKey}",
                TestData.GhostResponseJson);
            var repository = CreateRepository(mockHttp);

            // Act
            var actual = await repository.GetPagedPostsAsync(2, 5);

            var expected = new PagedPostCollection
            {
                CurrentPage = 2,
                PostsPerPage = 1,
                TotalPosts = 7,
                Posts = new[] {TestData.BlogPost}
            };

            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.TotalPosts, actual.TotalPosts);
            Assert.Equal(expected.PostsPerPage, actual.PostsPerPage);
            Assert.Equal(expected.Posts, actual.Posts);
        }
        
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsEmptyCollection_WhenHttpClientThrowsException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(HttpMethod.Get, BaseEndpoint)
                .Throw(new InvalidOperationException());

            var repository = CreateRepository(mockHttp);

            // Act
            var actual = await repository.GetPagedPostsAsync(1, 5);
            
            Assert.Equal(0, actual.CurrentPage);
            Assert.Equal(0, actual.TotalPosts);
            Assert.Equal(5, actual.PostsPerPage);
            Assert.Equal(Array.Empty<BlogPost>(), actual.Posts);
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsCorrectly()
        {
            var mockHttp = CreateMockHttp(
                $"{BaseEndpoint}/slug/welcome?fields=title,slug,html,published_at&key={ContentApiKey}",
                TestData.GhostResponseJson);
            var repository = CreateRepository(mockHttp);

            // Act
            var actual = await repository.GetPostAsync("welcome");

            var expected = TestData.BlogPost;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsNull_WhenHttpClientThrowsException()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(HttpMethod.Get, BaseEndpoint)
                .Throw(new InvalidOperationException());

            var repository = CreateRepository(mockHttp);

            // Act
            var actual = await repository.GetPostAsync("slug");

            Assert.Null(actual);
        }
        
        private MockHttpMessageHandler CreateMockHttp(
            string expectedEndpoint,
            string response)
        {
            var mockHttp = new MockHttpMessageHandler(); 
            mockHttp.Expect(
                    HttpMethod.Get,
                    expectedEndpoint)
                .Respond("application/json", response);

            return mockHttp;
        }

        private GhostBlogRepository CreateRepository(MockHttpMessageHandler mockHttp)
        {
            var httpClientFactory = CreateHttpClientFactory(mockHttp);
            var options = CreateOptions();
            var logger = Mock.Of<ILogger<GhostBlogRepository>>();

            return new GhostBlogRepository(options, httpClientFactory, logger);
        }
        
        private IHttpClientFactory CreateHttpClientFactory(MockHttpMessageHandler mockHttp)
        {
            return Mock.Of<IHttpClientFactory>(m =>
                m.CreateClient(It.IsAny<string>()) == mockHttp.ToHttpClient());
        }
        
        private IOptions<GhostOptions> CreateOptions()
        {
            return Mock.Of<IOptions<GhostOptions>>(m =>
                m.Value == new GhostOptions()
                {
                    ApiUrl = ApiUrl,
                    ContentApiKey = ContentApiKey
                });
        }
    }
}
