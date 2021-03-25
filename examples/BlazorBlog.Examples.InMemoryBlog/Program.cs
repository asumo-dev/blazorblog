using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Core.Services;
using BlazorBlog.Examples.InMemoryBlog.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorBlog.Extensions;
using BlazorBlog.UI;

namespace BlazorBlog.Examples.InMemoryBlog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(
                _ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            builder.Services.AddBlazorBlogCore();
            builder.Services.AddSingleton<IBlogRepository, InMemoryBlogRepository>();

            await builder.Build().RunAsync();
        }
    }
}