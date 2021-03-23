using System;
using System.Text.Json;
using BlazorBlog.Models;
using Xunit;

namespace BlazorBlog.Strapi.Tests
{
    public class PostContentTests
    {
        [Fact]
        public void ToBlogPost_CreatesBlogPost()
        {
            var postContent = new PostContent
            {
                Title = "Title",
                Slug = "slug",
                ContentMarkdown = "Hello\n\n![test.png](/uploads/test_7f18e0b271.png)",
                PublishedAt = new DateTime(2021, 1, 2, 3, 4, 5)
            };

            var actual = postContent.ToBlogPost("https://example.com/");

            var expected = new BlogPost(
                "Title",
                "slug",
                "<p>Hello</p><p><img src=\"https://example.com/uploads/test_7f18e0b271.png\" alt=\"test.png\" /></p>",
                new DateTime(2021, 1, 2, 3, 4, 5));

            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Slug, actual.Slug);
            // Remove line breaks so that it doesn't depend on a markdown parser
            Assert.Equal(expected.Body, actual.Body.Replace("\r", "").Replace("\n", ""));
            Assert.Equal(expected.PublishedAt, actual.PublishedAt);
        }
        
        [Fact]
        public void PostContent_CanBeDeserializedFromStrapiJson()
        {
            var strapiJson =
                @"{""id"":1,""title"":""Title"",""slug"":""slug"",""content"":""Hello\\n"",""published_at"":""2021-03-23T06:11:20.018Z"",""created_at"":""2021-03-23T06:11:10.154Z"",""updated_at"":""2021-03-23T06:11:20.035Z""}";

            var expected = new PostContent
            {
                Title = "Title",
                Slug = "slug",
                ContentMarkdown = "Hello\\n",
                PublishedAt = new DateTime(2021, 3, 23, 6, 11, 20, 18, DateTimeKind.Utc)
            };
            
            // Act
            var result = JsonSerializer.Deserialize<PostContent>(strapiJson);

            Assert.Equal(expected, result);
        }
    }
}
