using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Contentful;
using BlazorBlog.Extensions;
using BlazorBlog.UI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Examples.ContentfulBlog
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

            builder.Services.AddContentfulRepository(options =>
                builder.Configuration.Bind("Contentful", options));

            await builder.Build().RunAsync();
        }
    }
}
