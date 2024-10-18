using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions
{
	public class GetTransactionByIdEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app) =>
			app.MapGet("/{id}", HandleAsync)
			.WithName("Transactions: Get by Id")
			.WithSummary("Get a transaction")
			.WithDescription("Get a transaction")
			.WithOrder(4)
			.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
		{
			var request = new GetTransactionByIdRequest
			{
				UserId = "test@balta.io",
				Id = id
			};

			var result = await handler.GetByIdAsync(request);

			return Results.Json(result, statusCode: result._code);
		}
	}
}
