using System;
using BlazorBlog.Core.Models;

namespace BlazorBlog.GraphCms.Tests
{
    internal class TestData
    {
        public static readonly PostContent PostContent = new()
        {
            Title = "Title",
            Slug = "slug",
            Content = new PostContent.ContentContent
            {
                Html = "<p>Hello</p>"
            },
            PublishedAt = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero)
        };

        public static readonly PagedPostsResponse PagedPostsResponse = new()
        {
            PostsConnection = new PagedPostsResponse.PostsConnectionContent
            {
                Aggregate = new PagedPostsResponse.PostsConnectionContent.AggregateContent
                {
                    Count = 1
                },
                Edges = new []
                {
                    new PagedPostsResponse.PostsConnectionContent.EdgeContent
                    {
                        Node = PostContent
                    }
                }
            }
        };

        public static readonly BlogPost BlogPost =
            new("Title", "slug", "<p>Hello</p>",
                new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero).LocalDateTime);
    }
}
