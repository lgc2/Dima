using System.Globalization;
using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Reports;

public partial class IncomesAndExpensesPage : ComponentBase
{
    #region Properties

    protected List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "Receitas", Data = [] },
        new ChartSeries() { Name = "Despesas", Data = [] },
    };

    protected string[] XAxisLabels = [];

    #endregion
    
    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IReportHandler Handler { get; set; } = null!;

    #endregion
    
    protected override async Task OnInitializedAsync() => await GetIncomesAndExpensesAsync();

    private async Task GetIncomesAndExpensesAsync()
    {
        try
        {
            var result = await Handler.GetIncomesAndExpensesReportAsync(new GetIncomesAndExpansesRequest());

            if (!result.IsSuccess)
                return;
            
            if (result.Data == null)
                return;

            XAxisLabels = result.Data
                .Select(x => CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat.GetMonthName(x.Month))
                .ToArray();
            
            Series[0].Data = result.Data.Select(x => (double)x.Incomes).ToArray();
            Series[1].Data = result.Data.Select(x => (double)x.Expenses * -1).ToArray();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}