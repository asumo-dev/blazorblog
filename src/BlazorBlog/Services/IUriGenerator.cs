using BlazorBlog.Models;

namespace BlazorBlog.Services
{
    public interface IUriGenerator
    {
        string Post(BlogPost post);
        string Post(string slug);
        string Posts(int? page = null);
    }
}