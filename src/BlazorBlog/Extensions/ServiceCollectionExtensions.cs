using BlazorBlog.Services;
using BlazorBlog.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorBlogCore(this IServiceCollection services)
        {
            return services.AddSingleton<IBlogService, BlogService>()
                .AddSingleton<IUriGenerator, UriGenerator>();
        }
    }
}