using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class GhostBlogRepositoryTests
    {
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var getPostsAsyncReturn = Task.FromResult(TestData.PostsResponse);
            var client = Mock.Of<IGhostClient>(m =>
                m.GetPostsAsync(null, It.IsAny<NameValueCollection>()) == getPostsAsyncReturn);

            var subject = CreateRepository(client);

            // Act
            var actual = await subject.GetPagedPostsAsync(2, 5);

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
            var mockClient = new Mock<IGhostClient>();
            mockClient.Setup(m => m.GetPostsAsync(null, It.IsAny<NameValueCollection>()))
                .ThrowsAsync(new InvalidOperationException());

            var subject = CreateRepository(mockClient.Object);

            // Act
            var actual = await subject.GetPagedPostsAsync(1, 5);
            
            Assert.Equal(0, actual.CurrentPage);
            Assert.Equal(0, actual.TotalPosts);
            Assert.Equal(5, actual.PostsPerPage);
            Assert.Equal(Array.Empty<BlogPost>(), actual.Posts);
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsCorrectly()
        {
            var getPostsAsyncReturn = Task.FromResult(TestData.PostsResponse);
            var client = Mock.Of<IGhostClient>(m =>
                m.GetPostsAsync("welcome", It.IsAny<NameValueCollection>()) == getPostsAsyncReturn);

            var subject = CreateRepository(client);

            // Act
            var actual = await subject.GetPostAsync("welcome");

            var expected = TestData.BlogPost;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsNull_WhenHttpClientThrowsException()
        {
            var mockClient = new Mock<IGhostClient>();
            mockClient.Setup(m => m.GetPostsAsync("slug", It.IsAny<NameValueCollection>()))
                .ThrowsAsync(new InvalidOperationException());

            var subject = CreateRepository(mockClient.Object);

            // Act
            var actual = await subject.GetPostAsync("slug");

            Assert.Null(actual);
        }
        
        private GhostBlogRepository CreateRepository(IGhostClient client)
        {
            var logger = Mock.Of<ILogger<GhostBlogRepository>>();

            return new GhostBlogRepository(client, logger);
        }
    }
}
