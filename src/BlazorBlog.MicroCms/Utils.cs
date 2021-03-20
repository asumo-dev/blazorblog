using System;
using System.Collections.Specialized;
using System.Web;

namespace BlazorBlog.MicroCms
{
    public static class Utils
    {
        public static string BuildEndpoint(string endpoint, string? id = null, NameValueCollection? queryParams = null)
        {
            if (id != null)
            {
                endpoint = $"{endpoint.TrimEnd('/')}/{id}";
            }
            
            if (queryParams != null)
            {
                var builder = new UriBuilder(endpoint)
                {
                    Query = ToQueryString(queryParams)
                };
                
                // Remove port number
                endpoint = builder.Uri
                    .GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);
            }

            return endpoint;
        }

        private static string ToQueryString(NameValueCollection collection)
        {
            var httpValueCollection = HttpUtility.ParseQueryString(string.Empty);

            foreach (string key in collection)
            {
                httpValueCollection[key] = collection[key];
            }

            // ToString of NameValueCollection returned by HttpUtility.ParseQueryString generates a query string.
            return httpValueCollection.ToString() ?? string.Empty;
        }
    }
}