using System;
using System.Net.Http;
using BlazorBlog.Core.Services;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorBlog.Contentful
{
    public static class ServiceCollectionExtensions
    {
        private const string HttpClientName = "ContentfulHttpClient";

        public static IServiceCollection AddContentfulRepository(
            this IServiceCollection services, Action<ContentfulOptions> configureOptions)
        {
            services.AddLogging();
            services.AddHttpClient(HttpClientName);
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, ContentfulBlogRepository>();

            services.AddSingleton<IContentfulClient>((sp) =>
            {
                var options = sp.GetService<IOptions<ContentfulOptions>>()?.Value;
                var factory = sp.GetService<IHttpClientFactory>();
                return new ContentfulClient(factory?.CreateClient(HttpClientName), options);
            });

            return services;
        }
    }
}
