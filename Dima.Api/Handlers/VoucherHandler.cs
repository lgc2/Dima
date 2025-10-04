using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class VoucherHandler(AppDbContext context) : IVoucherHandler
{
	public async Task<Response<Voucher?>> GetByCodeAsync(GetVoucherByCodeRequest request)
	{
		try
		{
			var data = await context.Vouchers
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Code == request.Code && x.IsActive == true);

			return data is null
				? new Response<Voucher?>(null, 404, "Voucher not found")
				: new Response<Voucher?>(data);
		}
		catch
		{
			return new Response<Voucher?>(null, 500, "Something went wrong");
		}
	}
}
