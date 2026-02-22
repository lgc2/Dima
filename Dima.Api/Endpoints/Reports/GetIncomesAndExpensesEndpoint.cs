using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetIncomesAndExpensesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/incomes-and-expenses", HandleAsync)
            .WithName("Reports: Get Incomes And Expenses")
            .WithSummary("Get Incomes And Expenses")
            .WithDescription("Get Incomes And Expenses")
            .WithOrder(1)
            .Produces<Response<List<IncomesAndExpenses>?>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        var request = new GetIncomesAndExpansesRequest { UserId = user.Identity?.Name ?? String.Empty };

        var result = await handler.GetIncomesAndExpensesReportAsync(request);

        return Results.Json(result, statusCode: result.Code);
    }
}