using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Extensions;
using BlazorBlog.MicroCms;
using BlazorBlog.UI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Examples.MicroCms
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
            builder.Services.AddMicroCmsRepository(options =>
                builder.Configuration.Bind("MicroCms", options));

            await builder.Build().RunAsync();
        }
    }
}