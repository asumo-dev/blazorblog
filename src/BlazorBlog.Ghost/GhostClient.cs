using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorBlog.Core.Helpers;

namespace BlazorBlog.Ghost
{
    public class GhostClient : IGhostClient
    {
        private readonly string _postsEndpoint;
        private readonly string _contentApiKey;
        private readonly HttpClient _httpClient;

        public GhostClient(HttpClient httpClient, GhostOptions options)
        {
            options.ThrowsIfInvalid();

            _postsEndpoint = CreatePostsEndpoint(options.ApiUrl);
            _contentApiKey = options.ContentApiKey;
            _httpClient = httpClient;
        }

        public GhostClient(HttpClient httpClient, string apiUrl, string contentApiKey)
        {
            _postsEndpoint = CreatePostsEndpoint(apiUrl);
            _contentApiKey = contentApiKey;
            _httpClient = httpClient;
        }

        public async Task<PostsResponse?> GetPostsAsync(string? slug = null, NameValueCollection? @params = null)
        {
            var newParams = CloneParamsWithKey(@params);
            var endpoint = EndpointBuilder.Build(
                _postsEndpoint,
                slug != null ? $"slug/{slug}" : null,
                newParams);
            var response = await _httpClient.GetAsync(endpoint);

            CheckStatusCode(response.StatusCode);

            try
            {
                return await response.Content.ReadFromJsonAsync<PostsResponse>();
            }
            catch (JsonException e)
            {
                throw new InvalidOperationException("The response body was unknown format", e);
            }
        }

        private string CreatePostsEndpoint(string apiUrl)
            => $"{apiUrl.TrimEnd('/')}/ghost/api/v3/content/posts";

        private NameValueCollection CloneParamsWithKey(NameValueCollection? @params)
        {
            var newParams = @params != null
                ? new NameValueCollection(@params)
                : new NameValueCollection();

            newParams["key"] = _contentApiKey;

            return newParams;
        }

        private void CheckStatusCode(HttpStatusCode code)
        {
            if (code != HttpStatusCode.OK)
            {
                throw new InvalidOperationException($"The status code of the response was {code}.");
            }
        }
    }
}
