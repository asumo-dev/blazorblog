using System.Threading.Tasks;

namespace BlazorBlog.MicroCms
{
    public interface IMicroCmsClient
    {
        Task<MicroCmsCollection<TContent>?> GetContentsAsync<TContent>(MicroCmsQueryBuilder<TContent> queryBuilder);

        Task<TContent?> GetContentAsync<TContent>(MicroCmsQueryBuilder<TContent> queryBuilder);
    }
}
