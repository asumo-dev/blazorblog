using System;
using System.Linq;
using BlazorBlog.Core.Models;

namespace BlazorBlog.Ghost
{
    public static class Utils
    {
        public static PagedPostCollection ToPagedPostCollection(PostsResponse<PostContent> response)
        {
            if (response.Posts == null ||
                response.Meta?.Pagination?.Page == null ||
                response.Meta.Pagination.Limit == null ||
                response.Meta.Pagination.Total == null)
                throw new InvalidOperationException(
                    "PostsResponse with null Posts or Meta cannot be converted to BlogPost.");

            return new PagedPostCollection
            {
                Posts = response.Posts.Select(p => p.ToBlogPost()).ToList().AsReadOnly(),
                CurrentPage = response.Meta.Pagination.Page.Value - 1,
                TotalPosts = response.Meta.Pagination.Total.Value,
                PostsPerPage = response.Meta.Pagination.Limit.Value
            };
        }
    }
}
