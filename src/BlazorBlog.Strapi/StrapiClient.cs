using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorBlog.Core.Helpers;

namespace BlazorBlog.Strapi
{
    public class StrapiClient : IStrapiClient
    {
        private readonly string _baseEndpoint;
        private readonly HttpClient _httpClient;

        public StrapiClient(HttpClient httpClient, StrapiOptions options)
        {
            options.ThrowsIfInvalid();

            _baseEndpoint = options.BaseEndpoint.TrimEnd('/');
            _httpClient = httpClient;
        }

        public StrapiClient(string baseEndpoint, HttpClient httpClient)
        {
            _baseEndpoint = baseEndpoint.TrimEnd('/');
            _httpClient = httpClient;
        }

        public async Task<IList<T>?> GetAsync<T>(StrapiQueryBuilder<T> queryBuilder)
        {
            var endpoint = _baseEndpoint + queryBuilder.Build();
            var response = await _httpClient.GetAsync(endpoint);

            CheckStatusCode(response.StatusCode);

            try
            {
                return await response.Content.ReadFromJsonAsync<IList<T>>();
            }
            catch (JsonException e)
            {
                throw new InvalidOperationException("The response body was unknown format", e);
            }
        }

        public async Task<int> CountAsync()
        {
            var endpoint = EndpointBuilder.Build(_baseEndpoint, "count");
            var response = await _httpClient.GetAsync(endpoint);

            CheckStatusCode(response.StatusCode);
            
            var body = await response.Content.ReadAsStringAsync();
            if (int.TryParse(body, out var count))
            {
                return count;
            }

            throw new InvalidOperationException($"The response body was unknown format ({body}).");
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
