using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace BlazorBlog.Strapi.Tests
{
    public class StrapiClientTests
    {
        private const string BaseEndpoint = "https://example.com/";

        [Fact]
        public async Task GetAsync_ReturnsCorrectly()
        {
            var actualStrapiResponse =
                @"[{""id"":1,""title"":""Title"",""slug"":""slug"",""content"":""Hello\\n"",""published_at"":""2021-03-23T06:11:20.018Z"",""created_at"":""2021-03-23T06:11:10.154Z"",""updated_at"":""2021-03-23T06:11:20.035Z""}]";
            var mockHttp = CreateMockHttp("https://example.com?_start=1", actualStrapiResponse);
            
            var strapiClient = new StrapiClient(BaseEndpoint, mockHttp.ToHttpClient());
            var queryBuilder = new StrapiQueryBuilder<PostContent>()
                .Start(1);

            // Act
            var postContents = await strapiClient.GetAsync(queryBuilder);

            Assert.Equal(new[]
                {
                    new PostContent
                    {
                        Title = "Title",
                        Slug = "slug",
                        ContentMarkdown = "Hello\\n",
                        PublishedAt = new DateTime(2021, 3, 23, 6, 11, 20, 18, DateTimeKind.Utc)
                    }
                },
                postContents);
        }

        [Fact]
        public async Task GetAsync_ThrowsException_WhenApiReturnsInvalidData()
        {
            var invalidResponse = "123";
            var mockHttp = CreateMockHttp("https://example.com", invalidResponse);
            var strapiClient = new StrapiClient(BaseEndpoint, mockHttp.ToHttpClient());
            var queryBuilder = Mock.Of<StrapiQueryBuilder<PostContent>>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await strapiClient.GetAsync(queryBuilder);
            });
        }
        
        [Fact]
        public async Task CountAsync_RequestsToCorrectEndpoint()
        {
            var mockHttp = CreateMockHttp("https://example.com/count", "0");
            var strapiClient = new StrapiClient(BaseEndpoint, mockHttp.ToHttpClient());

            await strapiClient.CountAsync();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task CountAsync_ReturnsCorrectly()
        {
            var actualStrapiResponse = "10";
            var mockHttp = CreateMockHttp("https://example.com/count", actualStrapiResponse);
            var strapiClient = new StrapiClient(BaseEndpoint, mockHttp.ToHttpClient());

            var count = await strapiClient.CountAsync();

            Assert.Equal(10, count);
        }

        [Fact]
        public async Task CountAsync_ThrowsException_WhenApiReturnsInvalidData()
        {
            var invalidResponse = "{}";
            var mockHttp = CreateMockHttp("https://example.com/count", invalidResponse);
            var strapiClient = new StrapiClient(BaseEndpoint, mockHttp.ToHttpClient());

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await strapiClient.CountAsync();
            });
        }
        
        private MockHttpMessageHandler CreateMockHttp(string expectedEndpoint, string responseContent)
        {
            var mockHttp = new MockHttpMessageHandler(); 
            mockHttp.Expect(
                    HttpMethod.Get,
                    expectedEndpoint)
                .Respond("application/json", responseContent);

            return mockHttp;
        }
    }
}
