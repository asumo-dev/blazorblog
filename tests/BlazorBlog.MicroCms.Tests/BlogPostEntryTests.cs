using System;
using BlazorBlog.Models;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class BlogPostEntryTests
    {
        [Fact]
        public void ToBlogPostTest()
        {
            var entry = new BlogPostEntry
            {
                Id = "id123",
                Title = "Title",
                BodyHtml = "Hello",
                PublishedAt = new DateTime(2021, 1, 1)
            };

            var result = entry.ToBlogPost();
            
            Assert.Equal(
                new BlogPost("Title", "id123", "Hello", new DateTime(2021, 1, 1)),
                result);
        }
    }
}