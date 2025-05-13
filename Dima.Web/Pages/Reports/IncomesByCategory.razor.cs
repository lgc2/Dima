using System.Globalization;
using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Reports;

public partial class IncomesByCategoryPage : ComponentBase
{
    #region Properties
    
    protected readonly string Width = "100%";
    protected readonly string Height = "350px";

    protected ChartOptions ChartOptions = new()
    {
        YAxisFormat = "C2"
    };
    
    protected List<ChartSeries> Series = new List<ChartSeries>()
    {
        new ChartSeries() { Name = "Receitas", Data = [] },
    };

    protected string[] XAxisLabels = [];

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IReportHandler Handler { get; set; } = null!;

    #endregion
    
    protected override async Task OnInitializedAsync() => await GetIncomesByCategoryAsync();

    private async Task GetIncomesByCategoryAsync()
    {
        try
        {
            var result = await Handler.GetIncomesByCategoryReportAsync(new GetIncomesByCategoryRequest());

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add("Falha ao obter o relatório", Severity.Error);
                return;
            }

            XAxisLabels = result.Data.Select(x => x.Category).ToArray();
            
            Series[0].Data = result.Data.Select(x => (double)x.Incomes).ToArray();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}