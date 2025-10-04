using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class OrderHandler(AppDbContext context) : IOrderHandler
{
	public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
	{
		Order? order;

		try
		{
			order = await context.Orders
				.Include(x => x.Product)
				.Include(x => x.Voucher)
				.AsNoTracking()
				.FirstOrDefaultAsync(o => o.Id == request.Id && o.UserId == request.UserId);

			if (order == null) return new Response<Order?>(null, 404, "Order not found");

			if (order.Status != EOrderStatus.WaitingPayment)
				return new Response<Order?>(order, 400, "This order can not be canceled");

			order.Status = EOrderStatus.Canceled;
			order.UpdatedAt = DateTime.UtcNow;

			context.Orders.Update(order);
			await context.SaveChangesAsync();

			return new Response<Order?>(order, message: "Order canceled");
		}
		catch
		{
			return new Response<Order?>(null, 500, "Something went wrong");
		}
	}

	public Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
	{
		throw new NotImplementedException();
	}

	public Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
	{
		throw new NotImplementedException();
	}

	public Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
	{
		throw new NotImplementedException();
	}

	public Task<Response<Order?>> PayAsync(PayOrderRequest request)
	{
		throw new NotImplementedException();
	}

	public Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
	{
		throw new NotImplementedException();
	}
}
