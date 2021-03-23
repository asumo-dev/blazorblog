using System;
using BlazorBlog.Models;
using Xunit;

namespace BlazorBlog.Core.Tests.Models
{
    public class PagedPostCollectionTests
    {
        [Fact]
        public void PostsPerPage_Set0_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new PagedPostCollection
                {
                    PostsPerPage = 0
                };
            });
        }
        
        [Theory]
        [InlineData(0, 5, 0)]
        [InlineData(3, 5, 1)]
        [InlineData(5, 5, 1)]
        [InlineData(6, 5, 2)]
        [InlineData(123, 6, 21)]
        public void TotalPages_ReturnsCorrectValue(int totalPosts, int postsPerPage, int expected)
        {
            var pagedPosts = new PagedPostCollection
            {
                TotalPosts = totalPosts,
                PostsPerPage = postsPerPage,
            };
            
            Assert.Equal(expected, pagedPosts.TotalPages);
        }

        [Fact]
        public void HasPrevPage_CurrentPageGreaterThan0_ReturnsTrue()
        {
            var pagedPosts = new PagedPostCollection
            {
                TotalPosts = 100,
                PostsPerPage = 10,
                CurrentPage = 1
            };
            
            Assert.True(pagedPosts.HasPrevPage);
        }

        [Fact]
        public void HasPrevPage_CurrentPageNotGreaterThan0_ReturnsFalse()
        {
            var pagedPosts = new PagedPostCollection
            {
                TotalPosts = 100,
                PostsPerPage = 10,
                CurrentPage = 0
            };
            
            Assert.False(pagedPosts.HasPrevPage);
        }

        [Fact]
        public void HasNextPage_CurrentPageLessThanLastPage_ReturnsTrue()
        {
            var pagedPosts = new PagedPostCollection
            {
                TotalPosts = 100,
                PostsPerPage = 10,
                CurrentPage = 8
            };
            
            Assert.True(pagedPosts.HasNextPage);
        }

        [Fact]
        public void HasPrevPage_CurrentPageNotLessThanLastPage_ReturnsFalse()
        {
            var pagedPosts = new PagedPostCollection
            {
                TotalPosts = 100,
                PostsPerPage = 10,
                CurrentPage = 9
            };
            
            Assert.False(pagedPosts.HasNextPage);
        }

        [Fact]
        public void Empty_ReturnsCorrectly()
        {
            var actual = PagedPostCollection.Empty(5);
            
            Assert.Equal(0, actual.TotalPosts);
            Assert.Equal(0, actual.CurrentPage);
            Assert.Equal(5, actual.PostsPerPage);
            Assert.Equal(Array.Empty<BlogPost>(), actual.Posts);
        }
    }
}
