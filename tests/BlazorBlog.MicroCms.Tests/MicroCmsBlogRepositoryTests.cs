using System;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class MicroCmsBlogRepositoryTests
    {
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var getAsyncReturn = Task.FromResult(new MicroCmsCollection<BlogPostEntity>
            {
                Contents = new [] {TestData.BlogPostEntity},
                Limit = 5,
                Offset = 15,
                TotalCount = 16
            });
            var client = Mock.Of<IMicroCmsClient>(m =>
                m.GetContentsAsync(It.IsAny<MicroCmsQueryBuilder<BlogPostEntity>>())
                == getAsyncReturn);

            var subject = CreateMicroCmsBlogRepository(client);

            // Act
            var actual = await subject.GetPagedPostsAsync(3, 5);

            var expected = new PagedPostCollection
            {
                CurrentPage = 3,
                PostsPerPage = 5,
                TotalPosts = 16,
                Posts = new[] { TestData.BlogPost }
            };

            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.TotalPosts, actual.TotalPosts);
            Assert.Equal(expected.PostsPerPage, actual.PostsPerPage);
            Assert.Equal(expected.Posts, actual.Posts);
        }

        [Fact]
        public async Task GetPagedPostsAsync_ReturnsEmptyCollection_WhenMicroCmsClientThrowsException()
        {
            var mockClient = new Mock<IMicroCmsClient>();
            mockClient.Setup(m => m.GetContentsAsync(It.IsAny<MicroCmsQueryBuilder<BlogPostEntity>>()))
                .ThrowsAsync(new InvalidOperationException());

            var subject = CreateMicroCmsBlogRepository(mockClient.Object);

            // Act
            var actual = await subject.GetPagedPostsAsync(1, 5);

            Assert.Equal(0, actual.CurrentPage);
            Assert.Equal(0, actual.TotalPosts);
            Assert.Equal(5, actual.PostsPerPage);
            Assert.Equal(Array.Empty<BlogPost>(), actual.Posts);
        }

        [Fact]
        public async Task GetPostsAsync_ReturnsCorrectly()
        {
            var getAsyncReturn = Task.FromResult(TestData.BlogPostEntity);
            var client = Mock.Of<IMicroCmsClient>(m =>
                m.GetContentAsync(It.IsAny<MicroCmsQueryBuilder<BlogPostEntity>>())
                == getAsyncReturn);

            var subject = CreateMicroCmsBlogRepository(client);

            // Act
            var actual = await subject.GetPostAsync("id123");

            var expected = TestData.BlogPost;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetPostAsync_ReturnsNull_WhenHttpClientThrowsException()
        {
            var mockClient = new Mock<IMicroCmsClient>();
            mockClient.Setup(m => m.GetContentAsync(It.IsAny<MicroCmsQueryBuilder<BlogPostEntity>>()))
                .ThrowsAsync(new InvalidOperationException());

            var subject = CreateMicroCmsBlogRepository(mockClient.Object);

            // Act
            var actual = await subject.GetPostAsync("slug");

            Assert.Null(actual);
        }

        private MicroCmsBlogRepository CreateMicroCmsBlogRepository(IMicroCmsClient client)
        {
            var logger = Mock.Of<ILogger<MicroCmsBlogRepository>>();
            return new MicroCmsBlogRepository(client, logger);
        }
    }
}
