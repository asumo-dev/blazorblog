using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using BlazorBlog.Contentful;
using BlazorBlog.Extensions;
using BlazorBlog.Services;
using BlazorBlog.UI;
using Contentful.Core.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorBlog.Examples.ContentfulBlog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(
                sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            
            builder.Services.AddBlazorBlogCore();

            builder.Services.Configure<ContentfulOptions>(options =>
                builder.Configuration.Bind("Contentful", options));
            builder.Services.AddSingleton<IBlogRepository, ContentfulBlogRepository>();

            await builder.Build().RunAsync();
        }
    }
}