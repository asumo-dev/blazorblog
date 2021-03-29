using System;
using System.Threading.Tasks;
using BlazorBlog.Core.Models;
using Contentful.Core.Models;

namespace BlazorBlog.Contentful
{
    public record BlogPostEntry
    {
        public SystemProperties? Sys { get; set; }

        public string? Title { get; set; }

        public string? Slug { get; set; }

        public Document? Body { get; set; }

        /// <summary>
        /// Create a <see cref="BlogPost"/>
        /// </summary>
        /// <param name="converter">The instance of <see cref="IHtmlConverter"/> that is used to convert
        /// <see cref="Document"/> to HTML. If this parameter is null <see cref="HtmlConverter"/> is used.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Cannot create a <see cref="BlogPost"/> from a
        /// <see cref="BlogPostEntry"/> that any properties are null.</exception>
        public async Task<BlogPost> ToBlogPostAsync(IHtmlConverter? converter = null)
        {
            if (Sys == null || Title == null || Slug == null || Body == null)
                throw new InvalidOperationException(
                    "Entity fields cannot be null. Check your settings on Contentful website.");

            var html = await (converter ?? new HtmlConverter())
                .ConvertDocumentToHtmlAsync(Body);
            return new BlogPost(Title, Slug, html, Sys.CreatedAt ?? default);
        }
    }

    public interface IHtmlConverter
    {
        public Task<string> ConvertDocumentToHtmlAsync(Document document)
        {
            var htmlRenderer = new HtmlRenderer();
            return htmlRenderer.ToHtml(document);
        }
    }

    public class HtmlConverter : IHtmlConverter {}
}
