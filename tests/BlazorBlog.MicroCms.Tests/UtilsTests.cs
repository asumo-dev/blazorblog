using System.Collections.Specialized;
using Xunit;

namespace BlazorBlog.MicroCms.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void BuildEndpoint_EndpointOnly_ReturnsCorrectly()
        {
            Assert.Equal("https://example.com", Utils.BuildEndpoint("https://example.com"));
        }
        
        [Theory]
        [InlineData("https://example.com", "id123", "https://example.com/id123")]
        [InlineData("https://example.com/", "id123", "https://example.com/id123")]
        [InlineData("https://example.com/test", "id123", "https://example.com/test/id123")]
        public void BuildEndpoint_WithEndpointAndId_ReturnsCorrectly(string endpoint, string id, string expected)
        {
            Assert.Equal(expected, Utils.BuildEndpoint(endpoint, id));
        }
        
        [Theory]
        [InlineData("https://example.com")]
        [InlineData("https://example.com/")]
        public void BuildEndpoint_WithEndpointAndQueryParams_ReturnsCorrectly(string endpoint)
        {
            var result = Utils.BuildEndpoint(endpoint, queryParams: new NameValueCollection
            {
                {"id", "123"},
                {"name", "abc"}
            });
            
            Assert.Equal("https://example.com/?id=123&name=abc", result);
        }
        
        [Theory]
        [InlineData("https://example.com")]
        [InlineData("https://example.com/")]
        public void BuildEndpoint_WithAllParams_ReturnsCorrectly(string endpoint)
        {
            var result = Utils.BuildEndpoint(endpoint, "id123", new NameValueCollection
            {
                {"id", "123"},
                {"name", "abc"}
            });
            
            Assert.Equal("https://example.com/id123?id=123&name=abc", result);
        }
    }
}