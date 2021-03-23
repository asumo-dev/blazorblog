using System.Collections.Specialized;
using BlazorBlog.Helpers;
using Xunit;

namespace BlazorBlog.Core.Tests.Helpers
{
    public class EndpointBuilderTests
    {
        [Fact]
        public void Build_EndpointOnly_ReturnsCorrectly()
        {
            Assert.Equal("https://example.com", EndpointBuilder.Build("https://example.com"));
        }
        
        [Theory]
        [InlineData("https://example.com", "id123", "https://example.com/id123")]
        [InlineData("https://example.com/", "id123", "https://example.com/id123")]
        [InlineData("https://example.com/test", "id123", "https://example.com/test/id123")]
        public void Build_WithEndpointAndId_ReturnsCorrectly(string endpoint, string id, string expected)
        {
            Assert.Equal(expected, EndpointBuilder.Build(endpoint, id));
        }
        
        [Theory]
        [InlineData("https://example.com")]
        [InlineData("https://example.com/")]
        public void Build_WithEndpointAndQueryParams_ReturnsCorrectly(string endpoint)
        {
            var result = EndpointBuilder.Build(endpoint, queryParams: new NameValueCollection
            {
                {"id", "123"},
                {"name", "abc"}
            });
            
            Assert.Equal("https://example.com/?id=123&name=abc", result);
        }
        
        [Theory]
        [InlineData("https://example.com")]
        [InlineData("https://example.com/")]
        public void Build_WithAllParams_ReturnsCorrectly(string endpoint)
        {
            var result = EndpointBuilder.Build(endpoint, "id123", new NameValueCollection
            {
                {"id", "123"},
                {"name", "abc"}
            });
            
            Assert.Equal("https://example.com/id123?id=123&name=abc", result);
        }
        
        [Fact]
        public void Build_GivenBaseUrlParamWithNonDefaultPort_ReturnsEndpointWithPort()
        {
            var actual = EndpointBuilder
                .Build("https://example.com:1234", "test", new NameValueCollection{ {"id", "123"}});
            Assert.Equal("https://example.com:1234/test?id=123", actual);
        }
    }
}
