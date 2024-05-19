using Domain.Entities;
using System.Threading;

namespace Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> RetrieveAllAsync(CancellationToken cancellationToken = default);
		Task<Article?> RetrieveAsync(Guid id, CancellationToken cancellationToken = default);
		void Insert(Article article);
        void Update(Article article);
    }
}
