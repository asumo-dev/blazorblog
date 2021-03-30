using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace BlazorBlog.GraphCms
{
    public class GraphCmsClient : IGraphCmsClient
    {
        private readonly GraphQLHttpClient _client;

        public GraphCmsClient(GraphCmsOptions options)
        {
            options.ThrowsIfInvalid();
            
            _client = new GraphQLHttpClient(options.Endpoint, new SystemTextJsonSerializer());

            if (!string.IsNullOrEmpty(options.ApiToken))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", options.ApiToken);
            }
        }
        
        public Task<GraphQLResponse<T>> SendQueryAsync<T>(string query, object variables)
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
