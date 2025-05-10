using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class ReportHandler(IHttpClientFactory httpClientFactory) : IReportHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpansesRequest request) =>
        await _client.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>("v1/reports/incomes-and-expenses")
        ?? new Response<List<IncomesAndExpenses>?>(null, 400, "Falha ao tentar retornar as receitas e despesas");

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request) =>
        await _client.GetFromJsonAsync<Response<List<IncomesByCategory>?>>("v1/reports/incomes")
        ?? new Response<List<IncomesByCategory>?>(null, 400, "Falha ao tentar retornar as receitas");

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request) =>
        await _client.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>("v1/reports/expenses")
        ?? new Response<List<ExpensesByCategory>?>(null, 400, "Falha ao tentar retornar as despesas");

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request) =>
        await _client.GetFromJsonAsync<Response<FinancialSummary?>>("v1/reports/summary")
        ?? new Response<FinancialSummary?>(null, 400, "Falha ao tentar retornar o resumo financeiro");
}
