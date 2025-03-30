using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class ListTransactionsPage : ComponentBase
{
	#region Properties

	public bool IsBusy { get; set; } = false;
	public List<Transaction> Transactions { get; set; } = [];
	public string SearchTerm { get; set; } = string.Empty;
	public int CurrentYear { get; set; } = DateTime.UtcNow.Year;
	public int CurrentMonth { get; set; } = DateTime.UtcNow.Month;
	public int[] Years { get; set; } =
	{
		DateTime.UtcNow.Year,
		DateTime.UtcNow.AddYears(-1).Year,
		DateTime.UtcNow.AddYears(-2).Year,
		DateTime.UtcNow.AddYears(-3).Year
	};

	#endregion

	#region Services

	[Inject]
	public ISnackbar Snackbar { get; set; } = null!;

	[Inject]
	public IDialogService DialogService { get; set; } = null!;

	[Inject]
	public ITransactionHandler Handler { get; set; } = null!;

	#endregion

	#region Overrides

	protected override async Task OnInitializedAsync() => await GetTransactions();

	#endregion

	#region PublicMethods

	public Func<Transaction, bool> Filter => (transaction) =>
	{
		if (string.IsNullOrEmpty(SearchTerm))
			return true;

		return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
		|| transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
	};

	public async Task OnDeleteButtonClickedAsync(long id, string title)
	{
		var result = await DialogService.ShowMessageBox("ATENÇÂO",
			$"Ao prosseguir, o lançamento \"{title}\" será excluído. Esta é uma ação irreversível! Deseja continuar?",
			yesText: "EXCLUIR",
			cancelText: "Cancelar");

		if (result is true)
			await OnDeleteAsync(id, title);

		StateHasChanged();
	}

	#endregion

	#region PrivateMethods

	private async Task GetTransactions()
	{
		IsBusy = true;

		try
		{
			var request = new GetTransationByPeriodRequest()
			{
				StartDate = DateTime.UtcNow.GetFirstDay(CurrentYear, CurrentMonth),
				EndDate = DateTime.UtcNow.GetLastDay(CurrentYear, CurrentMonth),
				PageNumber = 1,
				PageSize = 1000,
			};
			var result = await Handler.GetByPeriodAsync(request);
			if (result.IsSuccess)
				Transactions = result.Data ?? [];
		}
		catch (Exception ex)
		{
			Snackbar.Add(ex.Message, Severity.Error);
		}
		finally { IsBusy = false; }
	}

	private async Task OnDeleteAsync(long id, string title)
	{
		IsBusy = true;

		try
		{
			var result = await Handler.DeleteAsync(new DeleteTransactionRequest { Id = id });
			if (result.IsSuccess)
			{
				Snackbar.Add($"Lançamento \"{title}\" excluído.", Severity.Success);
				Transactions.RemoveAll(c => c.Id == id);
			}
			else
				Snackbar.Add(result.Message, Severity.Error);
		}
		catch (Exception ex)
		{
			Snackbar.Add(ex.Message, Severity.Error);
		}
		finally
		{
			IsBusy = false;
		}
	}

	#endregion


}
