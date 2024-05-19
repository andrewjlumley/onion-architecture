using Services;
using Contracts;
using Persistence.Repositories;

namespace Api.Endpoints;

public static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this IEndpointRouteBuilder app)
    {
		app.MapGet("api/articles", async (IServiceManager service) =>
		{
			var articles = await service.Articles.RetrieveAllAsync();
			return Results.Ok(articles);
		});

		app.MapGet("api/articles/{id}", async (Guid id, IServiceManager service) =>
		{
			var article = await service.Articles.RetrieveAsync(id);
			return Results.Ok(article);
		});

		app.MapPost("api/articles",async (ArticleCreateRequest request, IServiceManager service) =>
        {
            var article = await service.Articles.CreateAsync(
                request.Title,
                request.Content,
                request.Tags);

            return Results.Created($"api/articles/{article.Id}", article);
        });

		app.MapPut("api/articles/{id}", async (Guid id, IServiceManager service) =>
        {
            var article = await service.Articles.UpdateAsync(id);
			return Results.Ok(article);
		});
    }
}
