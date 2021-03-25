using System;
using BlazorBlog.Models;
using BlazorBlog.UI.Services;
using Xunit;

namespace BlazorBlog.UI.Tests.Services
{
    public class UriGeneratorTests
    {
        [Fact]
        public void PostTests()
        {
            var uriGenerator = new UriGenerator();
            var post = new BlogPost("title", "post-2", "body", new DateTime(2021, 1, 1));

            Assert.Equal("posts/post-1", uriGenerator.Post("post-1"));
            Assert.Equal("posts/post-2", uriGenerator.Post(post));
        }

        [Fact]
        public void PostsTest()
        {
            var uriGenerator = new UriGenerator();

            Assert.Equal("page/1", uriGenerator.Posts(1));
        }
    }
}