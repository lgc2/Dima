using Dima.Api.Data;
using Dima.Core.Common.Extensions;
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
			if (request.Type == Core.Enums.ETransactionType.Withdraw && request.Amount > 0)
				request.Amount *= -1;

			if (request.Type == Core.Enums.ETransactionType.Deposit && request.Amount <= 0)
				return new Response<Transaction?>(null, 400, "A Deposit must have the amount greater than zero");

			try
			{
				var category = await context
					.Categories
					.AsNoTracking()
					.FirstOrDefaultAsync(c => c.Id == request.CategoryId && c.UserId == request.UserId);

				if (category == null) return new Response<Transaction?>(null, 404, "Category informed was not found");

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
			if (request.Type == Core.Enums.ETransactionType.Withdraw && request.Amount > 0)
				request.Amount *= -1;

			if (request.Type == Core.Enums.ETransactionType.Deposit && request.Amount <= 0)
				return new Response<Transaction?>(null, 400, "A Deposit must have the amount greater than zero");

			try
			{
				var transaction = await context
					.Transactions
					.AsNoTracking()
					.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

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
				var transaction = await context
					.Transactions
					.AsNoTracking()
					.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

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

		public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
		{
			try
			{
				var transaction = await context
					.Transactions
					.AsNoTracking()
					.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);

				return transaction == null
					? new Response<Transaction?>(null, 404, "Transaction not found")
					: new Response<Transaction?>(transaction);
			}
			catch
			{
				return new Response<Transaction?>(null, 500, "Something went wrong");
			}
		}

		public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransationByPeriodRequest request)
		{
			try
			{
				request.StartDate ??= DateTime.UtcNow.GetFirstDay();
				request.EndDate ??= DateTime.UtcNow.GetLastDay();
			}
			catch
			{
				return new PagedResponse<List<Transaction>?>(null, 500, "It was not possible to get the start date or the end date");
			}

			var query = context
					.Transactions
					.AsNoTracking()
					.Where(t => t.UserId == request.UserId && (t.CreatedAt >= request.StartDate && t.CreatedAt <= request.EndDate));

			try
			{
				var transactions = await query
					.OrderByDescending(t => t.CreatedAt)
					.Skip((request.PageNumber - 1) * request.PageSize)
					.Take(request.PageSize)
					.ToListAsync();

				if (transactions == null) return new PagedResponse<List<Transaction>?>(null, 404, "Transactions not found");

				var count = await query.CountAsync();

				return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
			}
			catch
			{
				return new PagedResponse<List<Transaction>?>(null, 500, "Something went wrong");
			}
		}
	}
}
