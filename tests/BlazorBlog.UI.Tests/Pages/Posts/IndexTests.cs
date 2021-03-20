using System;
using System.Threading.Tasks;
using BlazorBlog.Models;
using BlazorBlog.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Index = BlazorBlog.UI.Pages.Posts.Index;

namespace BlazorBlog.UI.Tests.Pages.Posts
{
    public class IndexTests
    {
        private readonly TestContext _ctx;
        private readonly BlogPost _blogPost;
        
        public IndexTests()
        {
            _ctx = new TestContext();
            _blogPost = new BlogPost("Post #1", "post-1", "hello", new DateTime(2021, 1, 1));
            
            _ctx.Services.AddSingleton(
                Mock.Of<IBlogService>(m =>
                    m.GetPostAsync(It.IsAny<string>()) == Task.FromResult(_blogPost)));
        }

        [Fact]
        public void ServicesIsInjectedCorrectly()
        {
            var cut = _ctx.RenderComponent<Index>();
            
            Assert.Equal(_blogPost, cut.Instance.Post);
        }

        [Fact]
        public void IndexComponentRendersCorrectly()
        {
            var cut = _ctx.RenderComponent<Index>();
            
            cut.Find("h1").MarkupMatches("<h1>Post #1</h1>");
        }
    }
}