using Services;
using Contracts;
using Persistence.Repositories;
using Azure.Core;
using Common.Http;

namespace Api.Endpoints;

public static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this IEndpointRouteBuilder app)
    {
		app.MapGet("api/articles", async (HttpRequest request, IServiceManager service) =>
		{
			var articles = (await service.Articles.RetrieveAllAsync()).Select(f => f.AssignLinks(request));
			if (ODataHelper.TryQuery(request, out IODataPackage? package))
			{
				if (package?.Pagination != null)
				{
					// https://localhost:7195/api/articles?$top=2&$skip=2
					var paginated = new PaginatedResponse<ArticleResponse[]>(new Uri("http://localhost"), articles.Count(), articles.Skip(package!.Pagination!.Skip).Take(package.Pagination.Top).ToArray());
					return Results.Ok(paginated);
				}
			}
			return Results.Ok(articles);
		});

		app.MapGet("api/articles/{id}", async (HttpRequest request, Guid id, IServiceManager service) =>
		{
			var article = (await service.Articles.RetrieveAsync(id)).AssignLinks(request);
			return Results.Ok(article);
		});

		app.MapGet("api/articles/{id}/image", (Guid id, IServiceManager service) =>
		{
			var mimeType = "image/png";
			var path = @"C:\Users\andre\OneDrive\Pictures\lastsupper.png";
			return Results.File(path, contentType: mimeType);
		});

		app.MapPost("api/articles/{id}/image", async (Guid id, IFormFile file) =>
		{
			var tempFile = $"c:\\temp\\{id}{Path.GetExtension(file.FileName)}";
			using var stream = File.OpenWrite(tempFile);
			await file.CopyToAsync(stream);
		});

		app.MapPost("api/articles", async (ArticleRequest request, IServiceManager service) =>
        {
			var article = await service.Articles.CreateAsync(request);
            return Results.Created($"api/articles/{article.Id}", article);
        });

		app.MapPost("api/articles/{articleId}/comments", async (Guid articleId, ArticleCommentRequest request, IServiceManager service) =>
		{
			var comment = await service.Articles.CreateAsync(articleId, request);
			return Results.Created($"api/articles/{comment.ArticleId}/comments/{comment.Id}", comment);
		});

		app.MapPut("api/articles/{id}", async (Guid id, IServiceManager service) =>
        {
            var article = await service.Articles.UpdateAsync(id);
			return Results.Ok(article);
		});
    }

	private static ArticleResponse AssignLinks(this ArticleResponse response, HttpRequest request)
	{
		response.Self = new RestUri(new Uri($"{request.Scheme}://{request.Host}/{request.Path}"), HttpMethod.Get);
		return response;
	}
}
