using AutoMapper;
using Contracts;
using Domain.Entities.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article
{
	public class ArticleMappingProfile : Profile
	{
		public ArticleMappingProfile()
		{
			CreateMap<Domain.Entities.Article.Article, ArticleResponse>();
			CreateMap<ArticleRequest, Domain.Entities.Article.Article>();
			CreateMap<ArticleComment, ArticleCommentResponse>();
			CreateMap<ArticleCommentRequest, ArticleComment>();
		}
	}
}
