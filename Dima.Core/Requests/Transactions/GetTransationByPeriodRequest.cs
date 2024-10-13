namespace Dima.Core.Requests.Transactions;

public class GetTransationByPeriodRequest : PagedRequest
{
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
}
