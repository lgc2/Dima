using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetIncomesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/incomes", HandleAsync)
            .WithName("Reports: Get Incomes by Category")
            .WithSummary("Get Incomes by Category")
            .WithDescription("Get Incomes by Category")
            .WithOrder(2)
            .Produces<Response<List<IncomesByCategory>?>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        var request = new GetIncomesByCategoryRequest { UserId = user.Identity?.Name ?? String.Empty };

        var result = await handler.GetIncomesByCategoryReportAsync(request);

        return Results.Json(result, statusCode: result._code);
    }
}