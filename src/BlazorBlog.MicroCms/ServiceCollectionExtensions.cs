using System;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.MicroCms
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMicroCmsRepository(
            this IServiceCollection services, Action<MicroCmsOptions> configureOptions)
        {
            services.AddHttpClient<IBlogRepository, MicroCmsBlogRepository>();
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, MicroCmsBlogRepository>();

            return services;
        }
    }
}