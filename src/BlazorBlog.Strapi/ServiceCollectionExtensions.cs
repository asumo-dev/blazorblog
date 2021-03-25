using System;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Strapi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStrapiRepository(
            this IServiceCollection services, Action<StrapiOptions> configureOptions)
        {
            services.AddHttpClient<IBlogRepository, StrapiBlogRepository>();
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, StrapiBlogRepository>();

            return services;
        }
    }
}
