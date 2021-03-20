using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorBlog.Models;

namespace BlazorBlog.Services
{
    public class InMemoryBlogRepository : IBlogRepository
    {
        private readonly BlogPost[] _posts;

        public InMemoryBlogRepository()
        {
            var posts = new List<BlogPost>();
            for (var i = 0; i < 15; i++)
            {
                var body = $"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur ultrices, neque id varius convallis, erat felis commodo tortor, nec consequat justo nulla eu enim. Ut fringilla condimentum ligula, in tincidunt tortor malesuada quis. Donec eget ultrices libero. Fusce ac sem ac urna mattis egestas eget feugiat lectus. Morbi sed neque interdum, sodales nisl tincidunt, maximus felis. Proin malesuada augue nisl, eget condimentum lectus interdum vel. {i}";
                
                posts.Add(new BlogPost($"Post #{i}", $"slug-{i.ToString()}", body, new DateTime(2021, 1, i + 1)));
            }

            _posts = posts.ToArray();
        }

        public Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            if (page < 0) page = 0;
            
            // if the page is out of range, returns the last page
            var start = postsPerPage * page;
            if (start > _posts.Length)
            {
                page = (_posts.Length - 1) / postsPerPage;
                start = postsPerPage * page;
            }

            var end = start + postsPerPage;
            if (end > _posts.Length)
            {
                end = _posts.Length;
            }
            
            return Task.FromResult(new PagedPostCollection
            {
                Posts = _posts[start..end],
                CurrentPage = page,
                TotalPosts = _posts.Length,
                PostsPerPage = postsPerPage
            });
        }

        public Task<BlogPost?> GetPostAsync(string slug)
        {
            return Task.FromResult(_posts.FirstOrDefault(p => p.Slug == slug));
        }
    }
}