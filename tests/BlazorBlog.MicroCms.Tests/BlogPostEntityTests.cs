using System;
using BlazorBlog.Core.Models;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class BlogPostEntityTests
    {
        [Fact]
        public void ToBlogPostTest()
        {
            var entity = new BlogPostEntity
            {
                Id = "id123",
                Title = "Title",
                BodyHtml = "Hello",
                PublishedAt = new DateTime(2021, 1, 1)
            };

            var result = entity.ToBlogPost();
            
            Assert.Equal(
                new BlogPost("Title", "id123", "Hello", new DateTime(2021, 1, 1)),
                result);
        }
    }
}