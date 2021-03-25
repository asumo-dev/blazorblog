using System;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Ghost
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddGhostRepository(
            this IServiceCollection services, Action<GhostOptions> configureOptions)
        {
            services.AddLogging();
            services.AddHttpClient<IBlogRepository, GhostBlogRepository>();
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, GhostBlogRepository>();

            return services;
        }
    }
}
