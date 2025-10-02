namespace Dima.Core.Requests.Orders;

public class GetVoucherByCodeRequest : Request
{
	public string Code { get; set; } = string.Empty;
}
