using System;
using BlazorBlog.Core.Models;

namespace BlazorBlog.MicroCms.Tests
{
    internal static class TestData
    {
        public static readonly BlogPostEntity BlogPostEntity = new()
        {
            Id = "slug",
            Title = "Title",
            BodyHtml = "<p>Hello</p>",
            PublishedAt = new DateTime(2021, 1, 1)
        };

        public static readonly BlogPost BlogPost =
            new("Title", "slug", "<p>Hello</p>", new DateTime(2021, 1, 1));
    }
}
