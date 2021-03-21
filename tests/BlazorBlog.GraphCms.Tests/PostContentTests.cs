using System;
using BlazorBlog.Models;
using Xunit;

namespace BlazorBlog.GraphCms.Tests
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
                PublishedAt = new DateTime(2021, 1, 1),
                Content = new PostContent.ContentContent {Html = "<p>Hello</p>"}
            };
            
            Assert.Equal(
                new BlogPost("Title", "slug", "<p>Hello</p>", new DateTime(2021, 1, 1)),
                postContent.ToBlogPost());
        }
    }
}
