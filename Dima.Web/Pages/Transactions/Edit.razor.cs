using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Properties

    [Parameter] public string Id { get; set; } = String.Empty;
    public bool IsBusy { get; set; } = false;
    public UpdateTransactionRequest InputModel { get; set; } = new();
    public List<Category> Categories { get; set; } = [];

    #endregion

    #region Services

    [Inject] public ITransactionHandler TransactionHandler { get; set; } = null!;

    [Inject] public ICategoryHandler CategoryHandler { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        GetTransactionByIdRequest? request = null;

        try
        {
            request = new GetTransactionByIdRequest() { Id = long.Parse(Id) };
        }
        catch
        {
            Snackbar.Add("Parâmetro inválido", Severity.Error);
        }

        if (request is null)
            return;

        IsBusy = true;

        try
        {
            var response = await TransactionHandler.GetByIdAsync(request);

            if (response.IsSuccess && response.Data is not null)
            {
                InputModel = new UpdateTransactionRequest()
                {
                    Id = response.Data.Id,
                    Title = response.Data.Title,
                    Type = response.Data.Type,
                    Amount = response.Data.Amount,
                    CategoryId = response.Data.CategoryId,
                    PaidOrReceivedAt = response.Data.PaidOrReceivedAt,
                };
            }
            else
                Snackbar.Add("Não foi possível obter a transação.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }

        try
        {
            await GetCategoriesAsync();
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
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TransactionHandler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/lancamentos/historico");
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

    #region PrivateMethods

    private async Task GetCategoriesAsync()
    {
        var getAllCategoriesRequest = new GetAllCategoriesRequest();
        var result = await CategoryHandler.GetAllAsync(getAllCategoriesRequest);

        if (result.IsSuccess)
            Categories = result.Data ?? [];
        else
            Snackbar.Add(result.Message, Severity.Error);
    }

    #endregion
}