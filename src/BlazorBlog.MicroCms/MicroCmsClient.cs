using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBlog.MicroCms
{
    public class MicroCmsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _endpoint;

        public MicroCmsClient(string endpoint, string apiKey, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            _endpoint = endpoint;
        }
        
        public Task<T> GetAsync<T>(NameValueCollection? queryParams)
            => GetAsyncCore<T>(_endpoint, queryParams);

        public Task<T> GetAsync<T>(string id, NameValueCollection? queryParams)
        {
            return GetAsyncCore<T>(Utils.BuildEndpoint(_endpoint, id), queryParams);
        }

        private async Task<T> GetAsyncCore<T>(string endpoint, NameValueCollection? queryParams)
        {
            endpoint = Utils.BuildEndpoint(endpoint, queryParams: queryParams);
            var response = await _httpClient.GetAsync(endpoint);
            
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}