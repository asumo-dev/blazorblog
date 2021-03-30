using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Tests.Common;
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

            Expression<Func<GhostQueryBuilder<PostContent>, bool>> expectedQuery = b =>
                b.Build()
                    .AsQueryString()
                    .Contains(new NameValueCollection
                    {
                        {"fields", "title,slug,html,published_at"},
                        {"limit", "5"},
                        {"page", "3"},
                        {"order", "published_at DESC"},
                    });

            var client = Mock.Of<IGhostClient>(m =>
                m.GetPostsAsync(It.Is(expectedQuery)) == getPostsAsyncReturn);

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
        public async Task GetPagedPostsAsync_ReturnsEmptyCollection_WhenGhostClientThrowsException()
        {
            var mockClient = new Mock<IGhostClient>();
            mockClient.Setup(m => m.GetPostsAsync(It.IsAny<GhostQueryBuilder<PostContent>>()))
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

            Expression<Func<GhostQueryBuilder<PostContent>, bool>> expectedQuery = b =>
                b.Build().StartsWith("slug/welcome") &&
                b.Build().AsQueryString()
                    .Contains(new NameValueCollection
                    {
                        {"fields", "title,slug,html,published_at"},
                    });

            var client = Mock.Of<IGhostClient>(m =>
                m.GetPostsAsync(It.Is(expectedQuery)) == getPostsAsyncReturn);

            var subject = CreateRepository(client);

            // Act
            var actual = await subject.GetPostAsync("welcome");

            var expected = TestData.BlogPost;
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsNull_WhenGhostClientThrowsException()
        {
            var mockClient = new Mock<IGhostClient>();
            mockClient.Setup(m => m.GetPostsAsync(It.IsAny<GhostQueryBuilder<PostContent>>()))
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
