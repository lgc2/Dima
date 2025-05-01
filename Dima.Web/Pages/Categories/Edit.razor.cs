﻿using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories
{
	public partial class EditCategoryPage : ComponentBase
	{
		#region Properties

		public bool IsBusy { get; set; } = false;
		public UpdateCategoryRequest? InputModel { get; set; } = null;

		#endregion

		#region Parameters

		[Parameter]
		public string Id { get; set; } = string.Empty;

		#endregion

		#region Services

		[Inject]
		public ISnackbar Snackbar { get; set; } = null!;

		[Inject]
		public NavigationManager NavigationManager { get; set; } = null!;

		[Inject]
		public ICategoryHandler Handler { get; set; } = null!;

		#endregion

		#region Overrides

		protected override async Task OnInitializedAsync()
		{
			GetCategoryByIdRequest? request = null;

			try
			{
				request = new GetCategoryByIdRequest { Id = long.Parse(Id) };
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
				var response = await Handler.GetByIdAsync(request);

				if (response.IsSuccess && response.Data is not null)
					InputModel = new UpdateCategoryRequest
					{
						Id = response.Data.Id,
						Title = response.Data.Title,
						Description = response.Data.Description
					};
				else
					Snackbar.Add("Não foi possível obter a categoria.", Severity.Error);
			}
			catch (Exception ex)
			{
				Snackbar.Add(ex.Message, Severity.Error);
			}
			
			finally { IsBusy = false; }
		}

		#endregion

		#region Methods

		public async Task OnValidSubmitAsync()
		{
			if (InputModel is null) return;

			IsBusy = true;

			try
			{
				var result = await Handler.UpdateAsync(InputModel);

				if (result.IsSuccess)
				{
					Snackbar.Add("Categoria alterada com sucesso.", Severity.Success);
					NavigationManager.NavigateTo("/categorias");
				}
				else
					Snackbar.Add("Não foi possível alterar a categoria.", Severity.Error);
			}
			catch (Exception ex)
			{
				Snackbar.Add(ex.Message, Severity.Error);
			}
			finally { IsBusy = false; }
		}

		#endregion
	}
}
