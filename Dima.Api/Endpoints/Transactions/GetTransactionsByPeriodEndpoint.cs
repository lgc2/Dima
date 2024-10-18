using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core;
using Microsoft.AspNetCore.Mvc;
using Dima.Core.Requests.Transactions;

namespace Dima.Api.Endpoints.Transactions
{
	public class GetTransactionsByPeriodEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app) =>
			app.MapGet("/", HandleAsync)
			.WithName("Transactions: Get Transactions by period")
			.WithSummary("Get Transactions by period")
			.WithDescription("Get Transactions by period")
			.WithOrder(5)
			.Produces<PagedResponse<List<Transaction>?>>();

		private static async Task<IResult> HandleAsync(
			ITransactionHandler handler,
			[FromQuery] DateTime? startDate = null,
			[FromQuery] DateTime? endDate = null,
			[FromQuery] int pageNumber = Configuration.DefaultPageNumber,
			[FromQuery] int pageSize = Configuration.DefaultPageSize)
		{
			var request = new GetTransationByPeriodRequest
			{
				UserId = "test@balta.io",
				PageNumber = pageNumber,
				PageSize = pageSize,
				StartDate = startDate,
				EndDate = endDate,
			};

			var result = await handler.GetByPeriodAsync(request);

			return Results.Json(result, statusCode: result._code);
		}
	}
}
