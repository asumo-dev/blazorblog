using System.Threading.Tasks;
using GraphQL;

namespace BlazorBlog.GraphCms
{
    public interface IGraphCmsClient
    {
        Task<GraphQLResponse<T>> SendQueryAsync<T>(string query, object variables);
    }
}
