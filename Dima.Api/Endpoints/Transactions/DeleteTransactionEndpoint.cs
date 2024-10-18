﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions
{
	public class DeleteTransactionEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app) =>
			app.MapDelete("/{id}", HandleAsync)
			.WithName("Transactions: Delete")
			.WithSummary("Delete a transaction")
			.WithDescription("Delete a transaction")
			.WithOrder(3)
			.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
		{
			var request = new DeleteTransactionRequest
			{
				UserId = "test@balta.io",
				Id = id
			};

			var result = await handler.DeleteAsync(request);

			return Results.Json(result, statusCode: result._code);
		}
	}
}
