using System;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.GraphCms
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphCmsRepository(
            this IServiceCollection services, Action<GraphCmsOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, GraphCmsBlogRepository>();

            return services;
        }
    }
}
