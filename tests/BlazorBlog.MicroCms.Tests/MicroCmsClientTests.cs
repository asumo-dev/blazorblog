using System.Net.Http;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class MicroCmsClientTests
    {
        class FakeContent
        {
            public string? Id { get; set; } = null;
        }

        [Fact]
        public async Task GetContentAsync_ReturnsCorrectly()
        {
            const string apiUrl = "https://example.com";
            const string expectedEndpoint = apiUrl + "/test?fields=Id";
            const string apiKey = "https://example.com";

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, expectedEndpoint)
                .WithHeaders("X-API-KEY", apiKey)
                .Respond("application/json", "{ \"Id\": \"hello\" }");

            var queryBuilder = new MicroCmsQueryBuilder<FakeContent>()
                .ContentIdIs("test");
            var subject = new MicroCmsClient(mockHttp.ToHttpClient(), apiUrl, apiKey);

            var actual = await subject.GetContentAsync(queryBuilder);

            mockHttp.VerifyNoOutstandingRequest();
            Assert.Equal("hello", actual?.Id);
        }
    }
}
