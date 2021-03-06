using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Extensions;
using BlazorBlog.GraphCms;
using BlazorBlog.UI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Examples.GraphCmsBlog
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
            builder.Services.AddGraphCmsRepository(options => 
                builder.Configuration.Bind("GraphCMS", options));

            await builder.Build().RunAsync();
        }
    }
}