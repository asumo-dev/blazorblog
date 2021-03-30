using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using GraphQL;
using Moq;
using Xunit;

namespace BlazorBlog.GraphCms.Tests
{
    public class GraphCmsBlogRepositoryTests
    {
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var sendQueryAsyncReturn = Task.FromResult(new GraphQLResponse<PagedPostsResponse>
            {
                Data = TestData.PagedPostsResponse
            });

            var client = Mock.Of<IGraphCmsClient>(m =>
                m.SendQueryAsync<PagedPostsResponse>(It.IsAny<string>(), It.IsAny<object>()) == sendQueryAsyncReturn);

            var subject = CreateGraphCmsBlogRepository(client);

            // Act
            var actual = await subject.GetPagedPostsAsync(2, 5);

            var expected = new PagedPostCollection
            {
                CurrentPage = 2,
                PostsPerPage = 5,
                TotalPosts = 1,
                Posts = new[] {TestData.BlogPost}
            };

            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.TotalPosts, actual.TotalPosts);
            Assert.Equal(expected.PostsPerPage, actual.PostsPerPage);
            Assert.Equal(expected.Posts, actual.Posts);
        }

        [Fact]
        public async Task GetPostAsync_ReturnsCorrectly()
        {
            var sendQueryAsyncReturn = Task.FromResult(new GraphQLResponse<PostResponse>
            {
                Data = new PostResponse
                {
                    Post = TestData.PostContent
                }
            });

            var client = Mock.Of<IGraphCmsClient>(m =>
                m.SendQueryAsync<PostResponse>(It.IsAny<string>(), It.IsAny<object>()) == sendQueryAsyncReturn);

            var subject = CreateGraphCmsBlogRepository(client);

            // Act
            var actual = await subject.GetPostAsync("welcome");

            Assert.Equal(TestData.BlogPost, actual);
        }

        private GraphCmsBlogRepository CreateGraphCmsBlogRepository(IGraphCmsClient client)
        {
            return new(client);
        }
    }
}
