using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
	public static void Map(IEndpointRouteBuilder app) =>
		app.MapGet("/{id}", HandleAsync)
		.WithName("Categories: Get by Id")
		.WithSummary("Get a category")
		.WithDescription("Get a category")
		.WithOrder(4)
		.Produces<Response<Category?>>();

	private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
	{
		var request = new GetCategoryByIdRequest
		{
			UserId = "test@balta.io",
			Id = id
		};

		var result = await handler.GetByIdAsync(request);

		return Results.Json(result, statusCode: result._code);
	}
}
