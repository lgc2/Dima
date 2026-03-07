using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.IntegrationTests.Fixtures;

public class SeedDataTestFixture : TestFixtureBase
{
	public readonly CreateCategoryRequest CreateCategoryReq = new()
	{
		UserId = "test3@test.com",
		Title = $"Tech Lerning (title changed 1) {new Random().Next(0, 1000000)}",
		Description = $"Learning expanses (description changed 3) {new Random().Next(0, 1000000)}"
	};

	public Response<Category>? Category { get; private set; }

	public override async Task InitializeAsync()
	{
		await Login();
		Category = await CreateCategory();
	}

	private async Task Login()
	{
		var loginRequest = new LoginRequest
		{
			Email = "test3@test.com",
			Password = "Passw0rd@"
		};

		var loginResponse = await _accountClient.LoginAsync(loginRequest);
		Assert.Equal(200, (int)loginResponse!.StatusCode);
	}

	private async Task<Response<Category>?> CreateCategory() => await _categoriesClient.CreateAsync(CreateCategoryReq);
}
