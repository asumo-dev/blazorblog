using System;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using GraphQL;
using Microsoft.Extensions.Logging;

namespace BlazorBlog.GraphCms
{
    public class GraphCmsBlogRepository : IBlogRepository
    {
        private readonly IGraphCmsClient _client;
        private readonly ILogger<GraphCmsBlogRepository> _logger;

        private const string PostQuery = @"
query post($slug: String) {
  post(where: {slug: $slug}) {
    title
    slug
    content {
      html
    }
    publishedAt
  }
}
";

        private const string PagedPostsQuery = @"
query pagedPosts($skip: Int, $first: Int) {
  postsConnection(skip: $skip, first: $first) {
    aggregate {
      count
    }
    edges {
      node {
        title
        slug
        content {
          html
        }
        publishedAt
      }
    }
    pageInfo {
      pageSize
    }
  }
}
";

        public GraphCmsBlogRepository(IGraphCmsClient client, ILogger<GraphCmsBlogRepository> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var skip = postsPerPage * page;
            var first = postsPerPage;
            GraphQLResponse<PagedPostsResponse>? response = null;

            try
            {
                response = await _client.SendQueryAsync<PagedPostsResponse>(PagedPostsQuery, new {skip, first});
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from GraphCMS");
            }

            if (response?.Data.PostsConnection?.Edges == null ||
                response.Data.PostsConnection.Aggregate?.Count == null)
            {
                return PagedPostCollection.Empty(postsPerPage);
            }

            var posts = ToBlogPosts(response.Data.PostsConnection.Edges);
            if (posts == null)
            {
                return PagedPostCollection.Empty(postsPerPage);
            }

            return new PagedPostCollection
            {
                Posts = posts,
                CurrentPage = page,
                TotalPosts = response.Data.PostsConnection.Aggregate.Count.Value,
                PostsPerPage = postsPerPage
            };
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            try
            {
                var response = await _client.SendQueryAsync<PostResponse>(PostQuery, new {slug});

                return response.Data.Post?.ToBlogPost();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to fetch from GraphCMS");
                return null;
            }
        }

        private BlogPost[]? ToBlogPosts(PagedPostsResponse.PostsConnectionContent.EdgeContent[] edges)
        {
            var posts = new BlogPost[edges.Length];
            for (var i = 0; i < edges.Length; i++)
            {
                var post = edges[i].Node?.ToBlogPost();
                if (post == null)
                {
                    return null;
                }
                
                posts[i] = post;
            }

            return posts;
        }
    }
}
