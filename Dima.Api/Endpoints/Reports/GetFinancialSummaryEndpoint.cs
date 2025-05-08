using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetFinancialSummaryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/summary", HandleAsync)
            .WithName("Reports: Get Financial Summary")
            .WithSummary("Get Financial Summary")
            .WithDescription("Get Financial Summary")
            .WithOrder(4)
            .Produces<Response<FinancialSummary?>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        var request = new GetFinancialSummaryRequest { UserId = user.Identity?.Name ?? String.Empty };

        var result = await handler.GetFinancialSummaryReportAsync(request);

        return Results.Json(result, statusCode: result._code);
    }
}