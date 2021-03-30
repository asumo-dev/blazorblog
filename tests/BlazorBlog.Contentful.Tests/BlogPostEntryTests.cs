using System;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using Contentful.Core.Models;
using Moq;
using Xunit;

namespace BlazorBlog.Contentful.Tests
{
    public class BlogPostEntryTests
    {
        private readonly BlogPostEntry _subject;
        private readonly IHtmlConverter _htmlConverter;

        public BlogPostEntryTests()
        {
            _subject = new BlogPostEntry()
            {
                Title = "Title",
                Slug = "slug",
                Body = new Document(),
                Sys = new SystemProperties
                {
                    CreatedAt = new DateTime(2021, 1, 1)
                }
            };

            _htmlConverter = Mock.Of<IHtmlConverter>(m =>
                m.ConvertDocumentToHtmlAsync(It.IsAny<Document>()) == Task.FromResult("<p>Hello</p>"));
        }

        [Fact]
        public async Task ToBlogPostAsync_CreatesBlogPost()
        {
            var actual = await _subject.ToBlogPostAsync(_htmlConverter);

            var expected =
                new BlogPost("Title", "slug", "<p>Hello</p>", new DateTime(2021, 1, 1));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ToBlogPostAsync_Throws_IfAnyPropertiesAreNull()
        {
            var subject = _subject with
            {
                Body = null
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                subject.ToBlogPostAsync(_htmlConverter));

            subject = _subject with
            {
                Slug = null
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                subject.ToBlogPostAsync(_htmlConverter));

            subject = _subject with
            {
                Sys = null
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                subject.ToBlogPostAsync(_htmlConverter));
            subject = _subject with
            {
                Body = null
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                subject.ToBlogPostAsync(_htmlConverter));
        }
    }
}
