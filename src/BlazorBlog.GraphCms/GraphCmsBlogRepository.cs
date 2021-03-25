using System.Linq;
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

            return new PagedPostCollection
            {
                Posts = response.Data.PostsConnection.Edges
                    .Select(m => m.Node.ToBlogPost())
                    .ToList()
                    .AsReadOnly(),
                CurrentPage = page,
                TotalPosts = response.Data.PostsConnection.Aggregate.Count,
                PostsPerPage = postsPerPage
            };
        }

        public async Task<BlogPost?> GetPostAsync(string slug)
        {
            var response = await SendQueryAsync<PostResponse>(PostQuery, new {slug});

            return response.Data.Post?.ToBlogPost();
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
