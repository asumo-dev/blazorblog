using System;
using System.Text.Json.Serialization;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class MicroCmsQueryBuilderTests
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
            var builder1 = new MicroCmsQueryBuilder<FakeContent>();

            var actual1 = builder1
                .Limit(5)
                .Offset(3)
                .OrderByDescending(m => m.PublishedAt)
                .Build();

            Assert.Equal("?fields=Name%2cpublishedAt&limit=5&offset=3&order=-publishedAt", actual1);

            var builder2 = new MicroCmsQueryBuilder<FakeContent>();

            var actual2 = builder2
                .Limit(5)
                .Offset(3)
                .Order(m => m.PublishedAt)
                .Build();

            Assert.Equal("?fields=Name%2cpublishedAt&limit=5&offset=3&order=publishedAt", actual2);


            var builder3 = new MicroCmsQueryBuilder<FakeContent>();

            var actual3 = builder3
                .ContentIdIs("slug")
                .Build();

            Assert.Equal("slug?fields=Name%2cpublishedAt", actual3);
        }
    }
}
