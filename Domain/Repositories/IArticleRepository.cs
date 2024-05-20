using Domain.Entities.Article;
using System.Security.Principal;
using System.Threading;

namespace Domain.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> RetrieveArticleAllAsync(CancellationToken cancellationToken = default);
		Task<Article?> RetrieveArticleAsync(Guid id, CancellationToken cancellationToken = default);
		void InsertArticle(Article article);
        void UpdateArticle(Article article);
	}
}
