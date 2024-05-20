using Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Article
{
	public interface IArticleService
	{
		Task<IEnumerable<ArticleResponse>> RetrieveAllAsync(CancellationToken cancellationToken = default);
		Task<ArticleResponse> RetrieveAsync(Guid id, CancellationToken cancellationToken = default);
		Task<ArticleResponse> CreateAsync(ArticleRequest request, CancellationToken cancellationToken = default);
		Task<ArticleCommentResponse> CreateAsync(Guid articleId, ArticleCommentRequest request, CancellationToken cancellationToken = default);
		Task<ArticleResponse> UpdateAsync(Guid id, CancellationToken cancellationToken = default);
	}
}
