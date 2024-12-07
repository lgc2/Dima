using Dima.Core.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class RegisterPage : ComponentBase
{
	#region Depedencies
	[Inject]
	public ISnackbar Snackbar { get; set; } = null!;

	[Inject]
	public IAccountHandler Handler { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	[Inject]
	public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

	#endregion

	//public MudForm? MudForm { get; set; }
}
