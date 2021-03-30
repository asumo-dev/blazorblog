using System.Text.Json;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class PostsResponseTests
    {
        [Fact]
        public void PostContent_CanBeDeserializedFromStrapiJson()
        {
            // Act
            var actual = JsonSerializer.Deserialize<PostsResponse<PostContent>>(
                TestData.GhostResponseJson);

            var expected = TestData.PostsResponse;

            Assert.Equal(expected.Posts, actual?.Posts);
            Assert.Equal(expected.Meta, actual?.Meta);
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
