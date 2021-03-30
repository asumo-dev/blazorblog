using BlazorBlog.Core.Models;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void ToPagedPostCollection_CreatesPagedPostCollection()
        {
            var postsResponse = TestData.PostsResponse;

            var actual = Utils.ToPagedPostCollection(postsResponse);

            var expected = new PagedPostCollection
            {
                Posts = new [] {TestData.BlogPost},
                CurrentPage = 2,
                TotalPosts = 7,
                PostsPerPage = 1
            };

            Assert.Equal(expected.Posts, actual.Posts);
            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.TotalPages, actual.TotalPages);
            Assert.Equal(expected.PostsPerPage, actual.PostsPerPage);
        }
    }
}
