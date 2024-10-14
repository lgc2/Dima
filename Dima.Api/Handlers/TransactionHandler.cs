using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
	public class TransactionHandler(AppDbContext context) : ITransactionHandler
	{
		public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
		{
			try
			{
				var transaction = new Transaction
				{
					UserId = request.UserId,
					Title = request.Title,
					PaidOrReceivedAt = request.PaidOrReceivedAt,
					Type = request.Type,
					Amount = request.Amount,
					CategoryId = request.CategoryId
				};

				await context.Transactions.AddAsync(transaction);
				await context.SaveChangesAsync();

				return new Response<Transaction?>(transaction, 201, "Transaction created");
			}
			catch
			{
				return new Response<Transaction?>(null, 500, "Transaction not created");
			}
		}

		public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
		{
			try
			{
				var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

				if (transaction == null) return new Response<Transaction?>(null, 404, "Transaction not found");

				transaction.Title = request.Title;
				transaction.Type = request.Type;
				transaction.Amount = request.Amount;
				transaction.CategoryId = request.CategoryId;
				transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

				context.Transactions.Update(transaction);
				await context.SaveChangesAsync();

				return new Response<Transaction?>(transaction, 200, "Transaction updated");
			}
			catch
			{
				return new Response<Transaction?>(null, 500, "Transaction not updated");
			}
		}

		public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
		{
			try
			{
				var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

				if (transaction == null) return new Response<Transaction?>(null, 404, "Transaction not found");

				context.Transactions.Remove(transaction);
				await context.SaveChangesAsync();

				return new Response<Transaction?>(transaction, 200, "Transaction deleted");
			}
			catch
			{
				return new Response<Transaction?>(null, 500, "Transaction not deleted");
			}
		}

		public Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransationByPeriodRequest request)
		{
			throw new NotImplementedException();
		}
	}
}
