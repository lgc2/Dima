using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions
{
	public class CreateTransactionEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app) =>
			app.MapPost("/", HandleAsync)
			.WithName("Transactions: Create")
			.WithSummary("Create a new transaction")
			.WithDescription("Create a new transaction")
			.WithOrder(1)
			.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandleAsync(ITransactionHandler handler, CreateTransactionRequest request)
		{
			var result = await handler.CreateAsync(request);

			return Results.Json(result, statusCode: result._code);
		}
	}
}
