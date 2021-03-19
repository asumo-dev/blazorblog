using System;
using System.Threading.Tasks;
using BlazorBlog.Models;
using BlazorBlog.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Index = BlazorBlog.Pages.Index;

namespace BlazorBlog.Tests.Pages
{
    public class IndexTests
    {
        private readonly TestContext _ctx;
        private readonly PagedPostCollection _pagedPosts;

        public IndexTests()
        {
            _ctx = new TestContext();
            _pagedPosts = new PagedPostCollection
            {
                Posts = new[] {new BlogPost("Post #1", "post-1", "body", new DateTime(2021, 1, 1))},
                TotalPosts = 1,
                PostsPerPage = 5
            };
            
            _ctx.Services.AddSingleton(
                Mock.Of<IBlogService>(m =>
                    m.GetPagedPostsAsync(It.IsAny<int>(), It.IsAny<int>()) == Task.FromResult(_pagedPosts)));
            _ctx.Services.AddSingleton(Mock.Of<IUriGenerator>());
        }

        [Fact]
        public void ServicesIsInjectedCorrectly()
        {
            var cut = _ctx.RenderComponent<Index>();
            
            Assert.Equal(_pagedPosts, cut.Instance.PagedPosts);
        }

        [Fact]
        public void ChildComponentsAreRenderedCorrectly()
        {
            var cut = _ctx.RenderComponent<Index>();

            cut.Find("article > h2").MarkupMatches("<h2>Post #1</h2>");
        }
    }
}