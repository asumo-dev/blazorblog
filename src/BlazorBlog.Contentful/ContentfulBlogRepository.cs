using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Models;
using BlazorBlog.Services;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Search;
using Microsoft.Extensions.Options;

namespace BlazorBlog.Contentful
{
    public class ContentfulBlogRepository : IBlogRepository, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ContentfulClient _client;

        public ContentfulBlogRepository(IOptions<ContentfulOptions> options)
        {
            _httpClient = new HttpClient();
            _client = new ContentfulClient(_httpClient, options.Value);
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var builder = new QueryBuilder<BlogPostEntry>()
                .ContentTypeIs("blogPost")
                .Skip(postsPerPage * page)
                .Limit(postsPerPage);
            var entries = await _client.GetEntries(builder);
            
            var list = new List<BlogPost>();
            foreach (var entry in entries.Items)
            {
                list.Add(await entry.ToBlogPostAsync());
            }

            return new PagedPostCollection
            {
                Posts = list.AsReadOnly(),
                CurrentPage = page,
                TotalPosts = entries.Total,
                PostsPerPage = postsPerPage
            };
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            var builder = new QueryBuilder<BlogPostEntry>()
                .ContentTypeIs("blogPost")
                .FieldMatches(m => m.Slug, slug)
                .Limit(1);
            var entry = await _client.GetEntries(builder);
            
            var post = entry.Items.FirstOrDefault();
            if (post == null) return null;
            
            return await post.ToBlogPostAsync();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}