using System.Net.Http;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class MicroCmsBlogRepositoryTests
    {
        private const string ApiKey = "API_KEY";
        
        [Fact]
        public void GetPagedPostsAsync_RequestCorrectly()
        {
            var mockHttp = CreateMockHttp(
                "https://example.com/test?fields=title,id,body,publishedAt&limit=5&offset=15&orders=-publishedAt");
            var httpClientFactory = CreateHttpClientFactory(mockHttp);
            var options = CreateOptions();
            
            var repository = new MicroCmsBlogRepository(options, httpClientFactory);

            // Act
            repository.GetPagedPostsAsync(3, 5);

            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Fact]
        public void GetPostsAsync_RequestCorrectly()
        {
            var mockHttp = CreateMockHttp(
                "https://example.com/test/id123?fields=title,id,body,publishedAt");
            var httpClientFactory = CreateHttpClientFactory(mockHttp);
            var options = CreateOptions();

            var repository = new MicroCmsBlogRepository(options, httpClientFactory);

            // Act
            repository.GetPostAsync("id123");

            mockHttp.VerifyNoOutstandingExpectation();
        }
        
        private MockHttpMessageHandler CreateMockHttp(string expectedEndpoint)
        {
            var mockHttp = new MockHttpMessageHandler(); 
            mockHttp.Expect(
                    HttpMethod.Get,
                    expectedEndpoint)
                .WithHeaders("X-API-KEY", ApiKey)
                .Respond("application/json", "{}");

            return mockHttp;
        }

        private IHttpClientFactory CreateHttpClientFactory(MockHttpMessageHandler mockHttp)
        {
            return Mock.Of<IHttpClientFactory>(m =>
                m.CreateClient(It.IsAny<string>()) == mockHttp.ToHttpClient());
        }
        
        private IOptions<MicroCmsOptions> CreateOptions()
        {
            return Mock.Of<IOptions<MicroCmsOptions>>(m =>
                m.Value == new MicroCmsOptions
                {
                    ApiKey = ApiKey,
                    Endpoint = "https://example.com/test"
                });
        }
    }
}