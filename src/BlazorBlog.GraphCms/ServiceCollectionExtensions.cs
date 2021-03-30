using System;
using BlazorBlog.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlazorBlog.GraphCms
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphCmsRepository(
            this IServiceCollection services, Action<GraphCmsOptions> configureOptions)
        {
            services.AddLogging();
            services.Configure(configureOptions);
            services.AddSingleton<IBlogRepository, GraphCmsBlogRepository>();

            services.AddSingleton<IGraphCmsClient>((sp) =>
            {
                var options = sp.GetService<IOptions<GraphCmsOptions>>()?.Value;

                if (options == null)
                {
                    throw new InvalidOperationException($"{nameof(IOptions<GraphCmsOptions>)} is not injected.");
                }

                return new GraphCmsClient(options);
            });

            return services;
        }
    }
}
