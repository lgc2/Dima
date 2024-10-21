﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions
{
	public class UpdateTransactionEndpoint : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app) =>
			app.MapPut("/{id}", HandleAsync)
			.WithName("Transactions: Update")
			.WithSummary("Update a transaction")
			.WithDescription("Update a transaction")
			.WithOrder(2)
			.Produces<Response<Transaction?>>();

		private static async Task<IResult> HandleAsync(ITransactionHandler handler, UpdateTransactionRequest request, long id)
		{
			//request.UserId = "test@balta.io"; --> this prop is within body
			request.Id = id;

			var result = await handler.UpdateAsync(request);

			return Results.Json(result, statusCode: result._code);
		}
	}
}