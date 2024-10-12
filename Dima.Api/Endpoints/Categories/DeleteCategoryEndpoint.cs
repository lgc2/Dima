using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
	public static void Map(IEndpointRouteBuilder app) =>
		app.MapDelete("/{id}", HandleAsync)
		.WithName("Categories: Delete")
		.WithSummary("Delete a category")
		.WithDescription("Delete a category")
		.WithOrder(3)
		.Produces<Response<Category?>>();

	private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
	{
		var request = new DeleteCategoryRequest
		{
			UserId = "test@balta.io",
			Id = id
		};

		var result = await handler.DeleteAsync(request);

		return Results.Json(result, statusCode: result._code);
	}
}
