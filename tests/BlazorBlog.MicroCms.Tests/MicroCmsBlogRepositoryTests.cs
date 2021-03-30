using System.Collections.Specialized;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
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
                m.GetAsync<MicroCmsCollection<BlogPostEntity>>(It.IsAny<NameValueCollection>())
                == getAsyncReturn);

            var subject = new MicroCmsBlogRepository(client);

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
        public async Task GetPostsAsync_ReturnsCorrectly()
        {
            var getAsyncReturn = Task.FromResult(TestData.BlogPostEntity);
            var client = Mock.Of<IMicroCmsClient>(m =>
                m.GetAsync<BlogPostEntity>("id123", It.IsAny<NameValueCollection>())
                == getAsyncReturn);

            var subject = new MicroCmsBlogRepository(client);

            // Act
            var actual = await subject.GetPostAsync("id123");

            var expected = TestData.BlogPost;
            Assert.Equal(expected, actual);
        }
    }
}
