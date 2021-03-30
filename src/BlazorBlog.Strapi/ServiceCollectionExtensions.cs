using System;
using System.Net.Http;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorBlog.Strapi
{
    public static class ServiceCollectionExtensions
    {
        private const string HttpClientName = "StrapiHttpClient";

        public static IServiceCollection AddStrapiRepository(
            this IServiceCollection services, Action<StrapiOptions> configureOptions)
        {
            services.AddLogging();
            services.AddHttpClient(HttpClientName);
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, StrapiBlogRepository>();

            services.AddSingleton<IStrapiClient>((sp) =>
            {
                var options = sp.GetService<IOptions<StrapiOptions>>()?.Value;
                var httpClient = sp.GetService<IHttpClientFactory>()?.CreateClient();

                if (options == null)
                {
                    throw new InvalidOperationException($"{nameof(IOptions<StrapiOptions>)} is not injected.");
                }

                if (httpClient == null)
                {
                    throw new InvalidOperationException($"{nameof(IHttpClientFactory)} is not injected.");
                }

                return new StrapiClient(httpClient, options);
            });

            return services;
        }
    }
}
