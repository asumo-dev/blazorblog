using System;
using BlazorBlog.Core.Models;

namespace BlazorBlog.Ghost.Tests
{
    public static class TestData
    {
        public static readonly PostContent PostContent = new()
        {
            Title = "Start here for a quick overview of everything you need to know",
            Slug = "welcome",
            Html = "<p><strong>Hey there</strong>, welcome to your new home on the web! </p>",
            PublishedAt = new DateTimeOffset(2021, 3, 24, 15, 38, 18, TimeSpan.Zero).LocalDateTime
        };

        public static readonly PostsResponse<PostContent> PostsResponse = new()
        {
            Posts = new[] {PostContent},
            Meta = new PostsResponse<PostContent>.MetaContent
            {
                Pagination = new PostsResponse<PostContent>.MetaContent.PaginationContent()
                {
                    Limit = 1,
                    Next = 4,
                    Page = 3,
                    Pages = 7,
                    Prev = 2,
                    Total = 7
                }
            }
        };

        public static readonly BlogPost BlogPost = new(
            "Start here for a quick overview of everything you need to know",
            "welcome",
            "<p><strong>Hey there</strong>, welcome to your new home on the web! </p>",
            new DateTimeOffset(2021, 3, 24, 15, 38, 18, TimeSpan.Zero).LocalDateTime);

        public const string GhostResponseJson =
            @"{""posts"":[{""slug"":""welcome"",""title"":""Start here for a quick overview of everything you need to know"",""html"":""<p><strong>Hey there</strong>, welcome to your new home on the web! </p>"",""published_at"":""2021-03-24T15:38:18.000+00:00""}],""meta"":{""pagination"":{""page"":3,""limit"":1,""pages"":7,""total"":7,""next"":4,""prev"":2}}}";
    }
}
