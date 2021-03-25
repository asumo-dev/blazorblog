using System;

namespace BlazorBlog.Core.Models
{
    public record BlogPost(string Title, string Slug, string Body, DateTime PublishedAt);
}