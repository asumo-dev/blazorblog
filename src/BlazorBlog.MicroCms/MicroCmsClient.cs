using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsClient : IMicroCmsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public MicroCmsClient(HttpClient httpClient, MicroCmsOptions options)
            : this(httpClient, options.Endpoint, options.ApiKey){}

        public MicroCmsClient(HttpClient httpClient, string endpoint, string apiKey)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            _endpoint = endpoint;
        }

        public Task<MicroCmsCollection<TContent>?> GetContentsAsync<TContent>(
            MicroCmsQueryBuilder<TContent> queryBuilder)
            => GetAsyncCore<MicroCmsCollection<TContent>>(CreateEndpoint(queryBuilder));

        public Task<TContent?> GetContentAsync<TContent>(
            MicroCmsQueryBuilder<TContent> queryBuilder)
            => GetAsyncCore<TContent>(CreateEndpoint(queryBuilder));

        private string CreateEndpoint<T>(MicroCmsQueryBuilder<T> queryBuilder)
            => $"{_endpoint.TrimEnd('/')}/{queryBuilder.Build()}";

        private async Task<T?> GetAsyncCore<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
