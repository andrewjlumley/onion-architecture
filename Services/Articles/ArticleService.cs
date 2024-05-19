using AutoMapper;
using Common.Exceptions;
using Contracts;
using Domain.Entities;
using Domain.Repositories;
using Domain.Types;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Services.Articles;

public sealed class ArticleService : IArticleService
{
    private readonly IRepositoryManager _repository;
	private readonly IMapper _mapper;

    public ArticleService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

	public async Task<IEnumerable<ArticleResponse>> RetrieveAllAsync(CancellationToken cancellationToken = default)
    {
		var articles = await _repository.ArticleRepository.RetrieveAllAsync(cancellationToken);
		return _mapper.Map<IEnumerable<ArticleResponse>>(articles);
	}

	public async Task<ArticleResponse> RetrieveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repository.ArticleRepository.RetrieveAsync(id, cancellationToken);
        if (article is null)
			throw new NotFoundException("Article not found");

		return _mapper.Map<ArticleResponse>(article);
	}

	public async Task<ArticleResponse> CreateAsync(string title, string content, List<string> tags, CancellationToken cancellationToken = default)
    {
        var article = new Article
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = content,
            Tags = tags,
            CreatedOnUtc = DateTime.UtcNow
        };

		if (article.TryIsValid(out ValidationResult[] violations) == false)
			throw new InvalidException(violations);

		_repository.ArticleRepository.Insert(article);
		await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ArticleResponse>(article);
	}

    public async Task<ArticleResponse> UpdateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var article = await _repository.ArticleRepository.RetrieveAsync(id);
        if (article is null)
            throw new NotFoundException("Article not found");

        article.PublishedOnUtc = DateTime.UtcNow;

        _repository.ArticleRepository.Update(article);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ArticleResponse>(article);
	}
}
