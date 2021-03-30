using System;
using System.Text.Json.Serialization;
using Xunit;

namespace BlazorBlog.Strapi.Tests
{
    public class StrapiQueryBuilderTests
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
            var builder1 = new StrapiQueryBuilder<FakeContent>();

            var actual1 = builder1
                .Eq(m => m.Name, "hello")
                .Limit(5)
                .Start(1)
                .OrderByDescending(m => m.PublishedAt)
                .Build();

            Assert.Equal("?Name_eq=hello&_limit=5&_start=1&_sort=publishedAt%3aDESC", actual1);

            var builder2 = new StrapiQueryBuilder<FakeContent>();

            var actual2 = builder2
                .Eq(m => m.Name, "hello")
                .Limit(5)
                .Start(1)
                .Order(m => m.PublishedAt)
                .Build();

            Assert.Equal("?Name_eq=hello&_limit=5&_start=1&_sort=publishedAt%3aASC", actual2);
        }
    }
}
