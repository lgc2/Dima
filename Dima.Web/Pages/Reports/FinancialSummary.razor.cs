using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Reports;

public class FinancialSummaryPage : ComponentBase
{
    #region Properties

    protected readonly string Width = "100%";
    protected readonly string Height = "350px";

    protected double[] Data = new double[2];
    protected string[] Labels = ["Receitas", "Despesas"];
    protected string Total = String.Empty;

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IReportHandler Handler { get; set; } = null!;

    #endregion

    protected override async Task OnInitializedAsync() => await GetFinancialSummaryAsync();

    private async Task GetFinancialSummaryAsync()
    {
        try
        {
            var result = await Handler.GetFinancialSummaryReportAsync(new GetFinancialSummaryRequest());

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add("Falha ao obter o relatório", Severity.Error);
                return;
            }

            Data[0] = (double)result.Data.Incomes;
            Data[1] = -(double)result.Data.Expenses;
            Total = result.Data.Total.ToString("C2");
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}