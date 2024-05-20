using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Repositories
{
    public sealed class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

		private IQueryable<Article> Queryable()
		{
			return _dbContext.Articles.Include(f => f.Comments).AsQueryable();
		}

		public async Task<IEnumerable<Article>> RetrieveArticleAllAsync(CancellationToken cancellationToken = default)
		{
			return await Queryable().ToListAsync(cancellationToken);
		}

		public async Task<Article?> RetrieveArticleAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await Queryable().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
		}

		public void InsertArticle(Article article) => _dbContext.Articles.Add(article);

        public void UpdateArticle(Article article) => _dbContext.Update(article);
	}
}