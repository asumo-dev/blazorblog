using System;
using System.Collections.Generic;
using BlazorBlog.Core.Models;
using Contentful.Core.Models;

namespace BlazorBlog.Contentful.Tests
{
    internal static class TestData
    {
        public static readonly BlogPostEntry BlogPostEntry = new()
        {
            Title = "Title",
            Slug = "slug",
            Body = new Document
            {
                Content = new List<IContent>
                {
                    new Paragraph
                    {
                        Content = new List<IContent>
                        {
                            new Text
                            {
                                Value = "Hello"
                            }
                        }
                    }
                }
            },
            Sys = new SystemProperties
            {
                CreatedAt = new DateTime(2021, 1, 1)
            }
        };

        public static readonly BlogPost BlogPost = new(
            "Title",
            "slug",
            "<p>Hello</p>",
            new DateTime(2021, 1, 1));
    }
}
