using System;
using System.Net.Http;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorBlog.Ghost
{
    public static class ServiceCollectionExtensions
    {
        private const string HttpClientName = "GhostHttpClient";

        public static IServiceCollection AddGhostRepository(
            this IServiceCollection services, Action<GhostOptions> configureOptions)
        {
            services.AddLogging();
            services.AddHttpClient(HttpClientName);
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, GhostBlogRepository>();

            services.AddSingleton<IGhostClient>((sp) =>
            {
                var options = sp.GetService<IOptions<GhostOptions>>()?.Value;
                var httpClient = sp.GetService<IHttpClientFactory>()?.CreateClient();

                if (options == null)
                {
                    throw new InvalidOperationException($"{nameof(IOptions<GhostOptions>)} is not injected.");
                }

                if (httpClient == null)
                {
                    throw new InvalidOperationException($"{nameof(IHttpClientFactory)} is not injected.");
                }

                return new GhostClient(httpClient, options);
            });

            return services;
        }
    }
}
