using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace BlazorBlog.Strapi.Tests
{
    public class StrapiBlogRepositoryTests
    {
        private const string ActualStrapiResponse =
                @"[{""id"":1,""title"":""Title"",""slug"":""slug"",""content"":""Hello\n"",""published_at"":""2021-03-23T06:11:20.018Z"",""created_at"":""2021-03-23T06:11:10.154Z"",""updated_at"":""2021-03-23T06:11:20.035Z""}]";

        private readonly BlogPost _expectedBlogPost = new(
            "Title", "slug", "<p>Hello</p>\n", new DateTime(2021, 3, 23, 6, 11, 20, 18, DateTimeKind.Utc));

        private const string BaseUrl = "https://example.com";
        private const string ContentName = "test";
        private const string BaseEndpoint = BaseUrl + "/" + ContentName;
        
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(HttpMethod.Get, $"{BaseEndpoint}?_start=10&_limit=5&_sort=published_at:DESC")
                .Respond("application/json", ActualStrapiResponse);
            mockHttp
                .When(HttpMethod.Get, $"{BaseEndpoint}/count")
                .Respond("text/plain", "1");

            var repository = CreateRepository(mockHttp);

            // Act
            var actual = await repository.GetPagedPostsAsync(2, 5);

            var expected = new PagedPostCollection
            {
                CurrentPage = 2,
                PostsPerPage = 5,
                TotalPosts = 1,
                Posts = new[] {_expectedBlogPost}
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
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(HttpMethod.Get, $"{BaseEndpoint}?slug_eq=slug&_limit=1")
                .Respond("application/json", ActualStrapiResponse);

            var repository = CreateRepository(mockHttp);

            // Act
            var actual = await repository.GetPostAsync("slug");

            Assert.Equal(_expectedBlogPost, actual);
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

        private StrapiBlogRepository CreateRepository(MockHttpMessageHandler mockHttp)
        {
            var httpClientFactory = CreateHttpClientFactory(mockHttp);
            var options = CreateOptions();
            var logger = Mock.Of<ILogger<StrapiBlogRepository>>();

            return new StrapiBlogRepository(options, httpClientFactory, logger);
        }
        
        private IHttpClientFactory CreateHttpClientFactory(MockHttpMessageHandler mockHttp)
        {
            return Mock.Of<IHttpClientFactory>(m =>
                m.CreateClient(It.IsAny<string>()) == mockHttp.ToHttpClient());
        }
        
        private IOptions<StrapiOptions> CreateOptions()
        {
            return Mock.Of<IOptions<StrapiOptions>>(m =>
                m.Value == new StrapiOptions()
                {
                    BaseUrl = BaseUrl,
                    ContentName = ContentName
                });
        }
    }
}
