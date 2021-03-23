using System;
using System.Collections.Specialized;
using System.Web;

namespace BlazorBlog.MicroCms
{
    public static class EndpointBuilder
    {
        public static string Build(string baseUrl, string? additionalPath = null, NameValueCollection? queryParams = null)
        {
            if (additionalPath != null)
            {
                baseUrl = $"{baseUrl.TrimEnd('/')}/{additionalPath}";
            }
            
            if (queryParams != null)
            {
                var builder = new UriBuilder(baseUrl)
                {
                    Query = ToQueryString(queryParams)
                };
                
                // Remove port number
                baseUrl = builder.Uri
                    .GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped);
            }

            return baseUrl;
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