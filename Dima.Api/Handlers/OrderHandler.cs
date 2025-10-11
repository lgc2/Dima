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
		try
		{
			var order = await context.Orders
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

	public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
	{
		Product? product;

		try
		{
			product = await context.Products
				.AsNoTracking()
				.FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive == true);

			if (product is null) return new Response<Order?>(null, 400, "The Order Product was not found");

			context.Attach(product);
		}
		catch
		{
			return new Response<Order?>(null, 500, "It was not possible to get the product");
		}

		Voucher? voucher = null;

		try
		{
			if (request.VoucherId is not null && request.VoucherId > 0)
			{
				voucher = await context.Vouchers
				.AsNoTracking()
				.FirstOrDefaultAsync(v => v.Id == request.VoucherId && v.IsActive == true);

				if (voucher is null)
					return new Response<Order?>(null, 400, "The Order Voucher was not found or it is invalid");

				context.Attach(voucher);
			}
		}
		catch
		{
			return new Response<Order?>(null, 500, "It was not possible to get the voucher");
		}

		var order = new Order
		{
			UserId = request.UserId,
			Product = product,
			ProductId = request.ProductId,
			Voucher = voucher,
			VoucherId = request.VoucherId,
		};

		try
		{
			await context.Orders.AddAsync(order);
			await context.SaveChangesAsync();
		}
		catch
		{
			return new Response<Order?>(null, 500, "It was not possible to create the order");
		}

		try
		{
			if (voucher is not null)
			{
				voucher.IsActive = false;
				context.Vouchers.Update(voucher);
				await context.SaveChangesAsync();
			}
		}
		catch
		{
			return new Response<Order?>(
				null, 500, $"Order {order.Number} created successfully, but the Voucher could not be deactivated");
		}

		return new Response<Order?>(order, 201, $"Order {order.Number} created successfully");
	}

	public Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
	{
		throw new NotImplementedException();
	}

	public Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
	{
		throw new NotImplementedException();
	}

	public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
	{
		Order? order;

		try
		{
			order = await context.Orders
				//.Include(x => x.Product)
				//.Include(x => x.Voucher)
				.FirstOrDefaultAsync(o => o.Id == request.Id && o.UserId == request.UserId);

			if (order == null) return new Response<Order?>(null, 404, "Order not found");
		}
		catch
		{
			return new Response<Order?>(null, 500, "Something went wrong while trying to retrieve the order");
		}

		switch (order.Status)
		{
			case EOrderStatus.Canceled:
				return new Response<Order?>(order, 400, "This order has already been canceled and cannot be paid");
			case EOrderStatus.Paid:
				return new Response<Order?>(order, 400, "This order has already been paid");
			case EOrderStatus.Refunded:
				return new Response<Order?>(order, 400, "This order has already been refunded and cannot be paid");
			case EOrderStatus.WaitingPayment:
				break;
			default:
				return new Response<Order?>(order, 400, "Invalid order status");
		}

		order.Status = EOrderStatus.Paid;
		order.ExternalReference = request.ExternalReference;
		order.UpdatedAt = DateTime.UtcNow;

		try
		{
			context.Orders.Update(order);
			await context.SaveChangesAsync();
		}
		catch
		{
			return new Response<Order?>(order, 500, "Something went wrong while trying to update the order");
		}

		return new Response<Order?>(order, message: $"Order {order.Number} paid successfully");
	}

	public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
	{
		Order? order;

		try
		{
			order = await context.Orders
				//.Include(x => x.Product)
				//.Include(x => x.Voucher)
				.FirstOrDefaultAsync(o => o.Id == request.Id && o.UserId == request.UserId);

			if (order == null) return new Response<Order?>(null, 404, "Order not found");

		}
		catch
		{
			return new Response<Order?>(null, 500, "Something went wrong while trying to retrieve the order");

		}

		switch (order.Status)
		{
			case EOrderStatus.Canceled:
				return new Response<Order?>(order, 400, "This order has already been canceled and cannot be refunded");
			case EOrderStatus.Paid:
				break;
			case EOrderStatus.Refunded:
				return new Response<Order?>(order, 400, "This order has already been refunded");
			case EOrderStatus.WaitingPayment:
				return new Response<Order?>(order, 400, "This order has not been paid yet, so it cannot be refunded");
			default:
				return new Response<Order?>(order, 400, "Invalid order status");
		}

		if (DateTime.UtcNow.Date > order.CreatedAt.AddDays(7).Date)
			return new Response<Order?>(order, 400, "This order has up to seven days to be refunded");

		order.Status = EOrderStatus.Refunded;
		order.UpdatedAt = DateTime.UtcNow;

		try
		{
			context.Orders.Update(order);
			await context.SaveChangesAsync();
		}
		catch
		{
			return new Response<Order?>(order, 500, "Something went wrong while trying to update the order");
		}

		return new Response<Order?>(order, message: $"Order {order.Number} refunded successfully");
	}
}