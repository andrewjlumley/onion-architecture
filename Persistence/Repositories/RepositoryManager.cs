using Domain.Repositories;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Persistence.Repositories
{
	public sealed class RepositoryManager : IRepositoryManager
	{
		private readonly Lazy<IArticleRepository> _articleRepository;
		private readonly Lazy<IUnitOfWork> _unitOfWork;

		public RepositoryManager(ApplicationDbContext dbContext)
		{
			_articleRepository = new Lazy<IArticleRepository>(() => new ArticleRepository(dbContext));
			_unitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
		}

		public IArticleRepository ArticleRepository => _articleRepository.Value;
		public IUnitOfWork UnitOfWork => _unitOfWork.Value;
	}
}
