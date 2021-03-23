using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorBlog.Extensions;
using BlazorBlog.Strapi;
using BlazorBlog.UI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorBlog.Examples.StrapiBlog
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
            builder.Services.AddStrapiRepository(options => 
                builder.Configuration.Bind("Strapi", options));

            await builder.Build().RunAsync();
        }
    }
}
