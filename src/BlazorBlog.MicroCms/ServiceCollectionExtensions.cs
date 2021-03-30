using System;
using System.Net.Http;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorBlog.MicroCms
{
    public static class ServiceCollectionExtensions
    {
        private const string HttpClientName = "microCMSHttpClient";

        public static IServiceCollection AddMicroCmsRepository(
            this IServiceCollection services, Action<MicroCmsOptions> configureOptions)
        {
            services.AddLogging();
            services.AddHttpClient(HttpClientName);
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, MicroCmsBlogRepository>();

            services.AddSingleton<IMicroCmsClient>((sp) =>
            {
                var options = sp.GetService<IOptions<MicroCmsOptions>>()?.Value;
                var httpClient = sp.GetService<IHttpClientFactory>()?
                    .CreateClient(HttpClientName);

                if (options == null)
                {
                    throw new InvalidOperationException($"{nameof(IOptions<MicroCmsOptions>)} is not injected.");
                }

                if (httpClient == null)
                {
                    throw new InvalidOperationException($"{nameof(IHttpClientFactory)} is not injected.");
                }

                return new MicroCmsClient(httpClient, options);
            });

            return services;
        }
    }
}
