using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
	public static void Map(IEndpointRouteBuilder app) =>
		app.MapPut("/{id}", HandleAsync)
		.WithName("Categories: Update")
		.WithSummary("Update a category")
		.WithDescription("Update a category")
		.WithOrder(2)
		.Produces<Response<Category?>>();

	private static async Task<IResult> HandleAsync(ICategoryHandler handler, UpdateCategoryRequest request, long id)
	{
		//request.UserId = "test@balta.io"; --> this prop is within body
		request.Id = id;

		var result = await handler.UpdateAsync(request);

		return Results.Json(result, statusCode: result._code);
	}
}
