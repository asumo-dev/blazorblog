using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Tests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BlazorBlog.Strapi.Tests
{
    class TestData
    {
        public static readonly PostContent PostContent = new()
        {
            Title = "Title",
            Slug = "slug",
            ContentMarkdown = "Hello",
            PublishedAt = new DateTime(2021, 1, 1)
        };

        public static readonly BlogPost BlogPost = new(
            "Title", "slug", "<p>Hello</p>\n", new DateTime(2021, 1, 1));
    }
    public class StrapiBlogRepositoryTests
    {
        private const string BaseUrl = "https://example.com";
        private const string ContentName = "test";

        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var getAsyncReturn = Task.FromResult((IList<PostContent>)new [] {TestData.PostContent});
            var countAsyncReturn = Task.FromResult(1);

            Expression<Func<StrapiQueryBuilder<PostContent>, bool>> expectedQuery = b =>
                b.Build()
                    .AsQueryString()
                    .Contains(new NameValueCollection
                    {
                        {"_start", "10"},
                        {"_limit", "5"},
                        {"_sort", "published_at:DESC"},
                    });

            var strapiClient = Mock.Of<IStrapiClient>(m =>
                m.GetAsync(It.Is(expectedQuery)) == getAsyncReturn &&
                m.CountAsync() == countAsyncReturn);

            var subject = CreateRepository(strapiClient);

            // Act
            var actual = await subject.GetPagedPostsAsync(2, 5);

            var expected = new PagedPostCollection
            {
                CurrentPage = 2,
                PostsPerPage = 5,
                TotalPosts = 1,
                Posts = new[] {TestData.BlogPost}
            };

            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.TotalPosts, actual.TotalPosts);
            Assert.Equal(expected.PostsPerPage, actual.PostsPerPage);
            Assert.Equal(expected.Posts, actual.Posts);
        }
        
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsEmptyCollection_WhenStrapiClientThrowsException()
        {
            var mockClient = new Mock<IStrapiClient>();
            mockClient.Setup(m => m.GetAsync(It.IsAny<StrapiQueryBuilder<PostContent>>()))
                .ThrowsAsync(new InvalidOperationException())
                .Verifiable();

            var subject = CreateRepository(mockClient.Object);

            // Act
            var actual = await subject.GetPagedPostsAsync(1, 5);
            
            Assert.Equal(0, actual.CurrentPage);
            Assert.Equal(0, actual.TotalPosts);
            Assert.Equal(5, actual.PostsPerPage);
            Assert.Equal(Array.Empty<BlogPost>(), actual.Posts);
            mockClient.Verify();
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsCorrectly()
        {
            var getAsyncReturn = Task.FromResult((IList<PostContent>)new [] {TestData.PostContent});
            var countAsyncReturn = Task.FromResult(1);

            Expression<Func<StrapiQueryBuilder<PostContent>, bool>> expectedQuery = b =>
                b.Build()
                    .AsQueryString()
                    .Contains(new NameValueCollection
                    {
                        {"slug_eq", "welcome"},
                        {"_limit", "1"}
                    });

            var strapiClient = Mock.Of<IStrapiClient>(m =>
                m.GetAsync(It.Is(expectedQuery)) == getAsyncReturn &&
                m.CountAsync() == countAsyncReturn);

            var subject = CreateRepository(strapiClient);

            // Act
            var actual = await subject.GetPostAsync("welcome");

            Assert.Equal(TestData.BlogPost, actual);
        }
        
        [Fact]
        public async Task GetPostAsync_ReturnsNull_WhenStrapiClientThrowsException()
        {
            var mockClient = new Mock<IStrapiClient>();
            mockClient.Setup(m => m.GetAsync(It.IsAny<StrapiQueryBuilder<PostContent>>()))
                .ThrowsAsync(new InvalidOperationException())
                .Verifiable();

            var subject = CreateRepository(mockClient.Object);

            // Act
            var actual = await subject.GetPostAsync("slug");

            Assert.Null(actual);
            mockClient.Verify();
        }

        private StrapiBlogRepository CreateRepository(IStrapiClient client)
        {
            var options = CreateOptions();
            var logger = Mock.Of<ILogger<StrapiBlogRepository>>();

            return new StrapiBlogRepository(client, options, logger);
        }

        private IOptions<StrapiOptions> CreateOptions()
        {
            return Mock.Of<IOptions<StrapiOptions>>(m =>
                m.Value == new StrapiOptions()
                {
                    BaseUrl = BaseUrl,
                    ContentName = ContentName
                });
        }
    }
}
