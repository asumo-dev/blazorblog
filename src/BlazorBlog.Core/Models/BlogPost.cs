using System;

namespace BlazorBlog.Models
{
    public record BlogPost(string Title, string Slug, string Body, DateTime PublishedAt);
}