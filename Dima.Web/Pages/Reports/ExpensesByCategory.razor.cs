using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Reports;

public partial class ExpensesByCategoryPage : ComponentBase
{
	#region Properties

	protected bool ThereIsDataToShow = false;

	protected readonly string Width = "100%";
    protected readonly string Height = "350px";

    protected double[] Data = [];
    protected string[] Labels = [];

    #endregion

    #region Services

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IReportHandler Handler { get; set; } = null!;

    #endregion
    
    protected override async Task OnInitializedAsync() => await GetExpensesByCategoryAsync();

    private async Task GetExpensesByCategoryAsync()
    {
        try
        {
            var result = await Handler.GetExpensesByCategoryReportAsync(new GetExpensesByCategoryRequest());

            if (!result.IsSuccess)
            {
                Snackbar.Add("Falha ao obter o relatório", Severity.Error);
                return;
			}

			if (result.Data is null || result.Data.Count == 0)
				return;

			ThereIsDataToShow = true;

			Data = result.Data.Select(x => (double)x.Expenses * -1).ToArray();
            Labels = result.Data.Select(x => x.Category).ToArray();
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}