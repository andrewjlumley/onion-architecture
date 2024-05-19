using Services.Articles;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Services
{
	public sealed class ServiceManager : IServiceManager
	{
		private readonly Lazy<IArticleService> _articleService;

		public ServiceManager(IRepositoryManager repository, IMapper mapper)
		{
			_articleService = new Lazy<IArticleService>(() => new ArticleService(repository, mapper));
		}

		public IArticleService Articles => _articleService.Value;
	}
}
