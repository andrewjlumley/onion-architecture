using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
	public interface IRepositoryManager
	{
		IArticleRepository ArticleRepository { get; }
		IUnitOfWork UnitOfWork { get; }
	}
}
