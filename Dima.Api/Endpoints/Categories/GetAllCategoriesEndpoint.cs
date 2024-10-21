﻿using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
	public static void Map(IEndpointRouteBuilder app) =>
	app.MapGet("/", HandleAsync)
	.WithName("Categories: Get all Categories")
	.WithSummary("Get all categories")
	.WithDescription("Get all categories")
	.WithOrder(5)
	.Produces<PagedResponse<List<Category>?>>();

	private static async Task<IResult> HandleAsync(
		ICategoryHandler handler,
		[FromQuery] int pageNumber = Configuration.DefaultPageNumber,
		[FromQuery] int pageSize = Configuration.DefaultPageSize)
	{
		var request = new GetAllCategoriesRequest
		{
			UserId = "test@balta.io",
			PageNumber = pageNumber,
			PageSize = pageSize
		};

		var result = await handler.GetAllAsync(request);

		return Results.Json(result, statusCode: result._code);
	}
}