using AutoMapper;
using Common.Exceptions;
using Contracts;
using Domain.Entities.Article;
using Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Services.Article;

public sealed class ArticleService : IArticleService
{
    private readonly IRepositoryManager _repository;
	private readonly IMapper _mapper;

    public ArticleService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

	private async Task<Domain.Entities.Article.Article> RetrieveArticle(Guid id, CancellationToken cancellationToken = default)
	{
		var article = await _repository.ArticleRepository.RetrieveArticleAsync(id, cancellationToken);
		if (article is null)
			throw new NotFoundException("Article not found");

		return article;
	}

	public async Task<IEnumerable<ArticleResponse>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
		var articles = await _repository.ArticleRepository.RetrieveArticleAllAsync(cancellationToken);
		return _mapper.Map<IEnumerable<ArticleResponse>>(articles);
	}

	public async Task<ArticleResponse> RetrieveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await RetrieveArticle(id, cancellationToken);
		return _mapper.Map<ArticleResponse>(article);
	}

    public async Task<ArticleResponse> CreateAsync(ArticleRequest request, CancellationToken cancellationToken = default)
    {
		var newArticle = Domain.Entities.Article.Article.Create();
		newArticle = _mapper.Map<ArticleRequest, Domain.Entities.Article.Article>(request, newArticle);

		if (newArticle.TryIsValid(out ValidationResult[] violations) == false)
            throw new InvalidException(violations);

		_repository.ArticleRepository.InsertArticle(newArticle);
	    await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ArticleResponse>(newArticle);
	}

	public async Task<ArticleCommentResponse> CreateAsync(Guid articleId, ArticleCommentRequest request, CancellationToken cancellationToken = default)
	{
		var article = await RetrieveArticle(articleId, cancellationToken);

		var newComment = ArticleComment.Create(articleId);
		article.Comments.Add(newComment);
		newComment = _mapper.Map<ArticleCommentRequest, ArticleComment>(request, newComment);

		if (newComment.TryIsValid(out ValidationResult[] violations) == false)
			throw new InvalidException(violations);

		await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ArticleCommentResponse>(newComment);
	}

	public async Task<ArticleResponse> UpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
		var article = await RetrieveArticle(id, cancellationToken);
		article.Published = DateTime.Now;

        _repository.ArticleRepository.UpdateArticle(article);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ArticleResponse>(article);
	}
}
