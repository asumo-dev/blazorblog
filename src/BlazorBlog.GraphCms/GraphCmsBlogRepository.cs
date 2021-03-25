using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using BlazorBlog.Core.Services;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Options;

namespace BlazorBlog.GraphCms
{
    public class GraphCmsBlogRepository : IBlogRepository
    {
        private readonly GraphQLHttpClient _client;

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

        public GraphCmsBlogRepository(IOptions<GraphCmsOptions> options)
        {
            _client = new GraphQLHttpClient(options.Value.Endpoint, new SystemTextJsonSerializer());

            if (!string.IsNullOrEmpty(options.Value.ApiToken))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.Value.ApiToken);
            }
        }

        public async Task<PagedPostCollection> GetPagedPostsAsync(int page, int postsPerPage)
        {
            var skip = postsPerPage * page;
            var first = postsPerPage;
            var response = await SendQueryAsync<PagedPostsResponse>(PagedPostsQuery, new {skip, first});

            if (response.Data.PostsConnection?.Edges == null ||
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
            var response = await SendQueryAsync<PostResponse>(PostQuery, new {slug});

            return response.Data.Post?.ToBlogPost();
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

        private Task<GraphQLResponse<T>> SendQueryAsync<T>(string query, object variables)
        {
            var request = new GraphQLHttpRequest
            {
                Query = query,
                Variables = variables
            };
            return _client.SendQueryAsync<T>(request);
        }
    }
}
