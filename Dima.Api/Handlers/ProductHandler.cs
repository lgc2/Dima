using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ProductHandler(AppDbContext context) : IProductHandler
{
	public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
	{
		var query = context.Products.AsNoTracking().Where(x => x.IsActive == true);

		try
		{
			var products = await query
				.OrderBy(p => p.Title)
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToListAsync();

			if (products == null || products.Count == 0)
				return new PagedResponse<List<Product>?>(null, 404, "Products not found");

			var count = await query.CountAsync();

			return new PagedResponse<List<Product>?>(products, count, request.PageNumber, request.PageSize);
		}
		catch
		{
			return new PagedResponse<List<Product>?>(null, 500, "Something went wrong");
		}
	}

	public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
	{
		try
		{
			var data = await context.Products
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Slug == request.Slug && x.IsActive == true);

			return data is null
				? new Response<Product?>(null, 404, "Product not found")
				: new Response<Product?>(data);
		}
		catch
		{
			return new Response<Product?>(null, 500, "Something went wrong");
		}
	}
}
