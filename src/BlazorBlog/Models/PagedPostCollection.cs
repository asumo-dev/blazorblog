using System;
using System.Collections.Generic;

namespace BlazorBlog.Models
{
    public class PagedPostCollection
    {
        private readonly int _postsPerPage;
        
        public int TotalPosts { get; init; }

        public int PostsPerPage
        {
            get => _postsPerPage;
            init
            {
                if(value <= 0) throw new ArgumentOutOfRangeException();
                
                _postsPerPage = value;
            }
        }

        public int CurrentPage { get; init; }

        public IReadOnlyList<BlogPost> Posts { get; init; } = default!;

        public int TotalPages => (TotalPosts + PostsPerPage - 1) / PostsPerPage;

        public bool HasPrevPage => CurrentPage > 0;

        public bool HasNextPage => CurrentPage < TotalPages - 1;
    }
}