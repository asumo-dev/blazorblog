using System.Threading.Tasks;

namespace BlazorBlog.Ghost
{
    public interface IGhostClient
    {
        Task<PostsResponse<T>?> GetPostsAsync<T>(GhostQueryBuilder<T>  queryBuilder);
    }
}
