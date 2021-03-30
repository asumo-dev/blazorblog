using System;
using System.Text.Json.Serialization;
using Xunit;

namespace BlazorBlog.Ghost.Tests
{
    public class GhostQueryBuilderTests
    {
        class FakeContent
        {
            public string? Name { get; set; }

            [JsonPropertyName("publishedAt")]
            public DateTime? PublishedAt { get; set; } = null;

            public string? Text = string.Empty;
        }

        [Fact]
        public void Works()
        {
            var builder1 = new GhostQueryBuilder<FakeContent>();

            var actual1 = builder1
                .Limit(5)
                .Page(2)
                .OrderByDescending(m => m.PublishedAt)
                .ApiKey("api_key")
                .Build();

            Assert.Equal("?fields=Name%2cpublishedAt&limit=5&page=2&order=publishedAt+DESC&key=api_key", actual1);

            var builder2 = new GhostQueryBuilder<FakeContent>();

            var actual2 = builder2
                .Limit(5)
                .Page(2)
                .Order(m => m.PublishedAt)
                .ApiKey("api_key")
                .Build();

            Assert.Equal("?fields=Name%2cpublishedAt&limit=5&page=2&order=publishedAt&key=api_key", actual2);


            var builder3 = new GhostQueryBuilder<FakeContent>();

            var actual3 = builder3
                .Slug("welcome")
                .ApiKey("api_key")
                .Build();

            Assert.Equal("slug/welcome?fields=Name%2cpublishedAt&key=api_key", actual3);
        }
    }
}
