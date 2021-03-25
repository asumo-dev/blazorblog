using System.Text.Json;
using BlazorBlog.Core.Models;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class PostsResponseTests
    {
        [Fact]
        public void PostContent_CanBeDeserializedFromStrapiJson()
        {
            // Act
            var actual = JsonSerializer.Deserialize<PostsResponse>(
                TestData.GhostResponseJson);

            var expected = TestData.PostsResponse;

            Assert.Equal(expected.Posts, actual?.Posts);
            Assert.Equal(expected.Meta, actual?.Meta);
        }

        [Fact]
        public void ToPagedPostCollection_CreatesPagedPostCollection()
        {
            var postsResponse = TestData.PostsResponse;

            var actual = postsResponse.ToPagedPostCollection();

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
        
        public class PostContentTests
        {
            [Fact]
            public void ToBlogPost_CreatesBlogPost()
            {
                var postContent = TestData.PostContent;

                var actual = postContent.ToBlogPost();

                var expected = TestData.BlogPost;
                Assert.Equal(expected, actual);
            }
        }
    }
}
