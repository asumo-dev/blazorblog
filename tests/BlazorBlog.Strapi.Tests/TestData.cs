using System;
using BlazorBlog.Core.Models;

namespace BlazorBlog.Strapi.Tests
{
    internal class TestData
    {
        public static readonly PostContent PostContent = new()
        {
            Title = "Title",
            Slug = "slug",
            ContentMarkdown = "Hello",
            PublishedAt = new DateTime(2021, 1, 1)
        };

        public static readonly BlogPost BlogPost = new(
            "Title", "slug", "<p>Hello</p>\n", new DateTime(2021, 1, 1));
    }
}
