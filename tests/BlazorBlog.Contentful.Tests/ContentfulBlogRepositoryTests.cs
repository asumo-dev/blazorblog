using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Tests.Common;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Moq;
using Xunit;

namespace BlazorBlog.Contentful.Tests
{
    public class ContentfulBlogRepositoryTests
    {
        [Fact]
        public async Task GetPagedPostsAsync_ReturnsCorrectly()
        {
            var getEntriesReturnedValue = Task.FromResult(
                new ContentfulCollection<BlogPostEntry>
                {
                    Items = new[] { TestData.BlogPostEntry },
                    Limit = 5,
                    Skip = 10,
                    Total = 11
                });

            Expression<Func<QueryBuilder<BlogPostEntry>, bool>> expectedQueryBuilder = (b) =>
                b.Build()
                    .AsQueryString()
                    .Contains(new NameValueCollection()
                    {
                        {"content_type", "blogPost"},
                        {"skip", "10"},
                        {"limit", "5"},
                    });
            var client = Mock.Of<IContentfulClient>(m =>
                m.GetEntries(It.Is(expectedQueryBuilder), It.IsAny<CancellationToken>())
                == getEntriesReturnedValue);
            var subject = new ContentfulBlogRepository(client);

            // Act
            var actual = await subject.GetPagedPostsAsync(2, 5);

            var expected = new PagedPostCollection
            {
                CurrentPage = 2,
                PostsPerPage = 5,
                TotalPosts = 11,
                Posts = new[] { TestData.BlogPost }
            };

            Assert.Equal(expected.CurrentPage, actual.CurrentPage);
            Assert.Equal(expected.TotalPosts, actual.TotalPosts);
            Assert.Equal(expected.PostsPerPage, actual.PostsPerPage);
            Assert.Equal(expected.Posts, actual.Posts);
        }


        [Fact]
        public async Task GetPostAsync_ReturnsCorrectly()
        {
            var getEntriesReturnedValue = Task.FromResult(
                new ContentfulCollection<BlogPostEntry>
                {
                    Items = new[] { TestData.BlogPostEntry },
                    Limit = 1,
                    Skip = 0,
                    Total = 1
                });

            Expression<Func<QueryBuilder<BlogPostEntry>, bool>> expectedQueryBuilder = (b) =>
                b.Build()
                    .AsQueryString()
                    .Contains(new NameValueCollection()
                    {
                        {"content_type", "blogPost"},
                        {"fields.slug[match]", "welcome"},
                        {"limit", "1"},
                    });
            var client = Mock.Of<IContentfulClient>(m =>
                m.GetEntries(It.Is(expectedQueryBuilder), It.IsAny<CancellationToken>())
                == getEntriesReturnedValue);
            var subject = new ContentfulBlogRepository(client);

            // Act
            var actual = await subject.GetPostAsync("welcome");

            var expected = TestData.BlogPost;
            Assert.Equal(expected, actual);
        }
    }
}
