using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/expenses", HandleAsync)
            .WithName("Reports: Get Expenses by Category")
            .WithSummary("Get Expenses by Category")
            .WithDescription("Get Expenses by Category")
            .WithOrder(3)
            .Produces<Response<List<ExpensesByCategory>?>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        var request = new GetExpensesByCategoryRequest { UserId = user.Identity?.Name ?? String.Empty };

        var result = await handler.GetExpensesByCategoryReportAsync(request);

        return Results.Json(result, statusCode: result._code);
    }
}