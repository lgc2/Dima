using Dima.Api.IntegrationTests.Clients;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests;

public class CategoriesIntegrationTests
{
	private readonly AccountClient _accountClient;
	private readonly CategoriesClient _categoriesClient;

	public CategoriesIntegrationTests()
	{
		_accountClient = new AccountClient();
		_categoriesClient = new CategoriesClient();
	}

	[Fact]
	public async Task GetByIdEndpoint_ReturnsSuccess()
	{
		await LoginAsync();

		var getByIdResponse = await _categoriesClient.GetByIdAsync(10020);
		Assert.Equal(200, getByIdResponse!._code);
		Assert.Equal(10020, getByIdResponse!.Data!.Id);
		Assert.Equal("Tech Lerning (title changed 1)", getByIdResponse!.Data!.Title);
		Assert.Equal("Learning expanses (description changed 3)", getByIdResponse!.Data!.Description);
		Assert.Equal("test3@test.com", getByIdResponse!.Data!.UserId);
		Assert.Null(getByIdResponse.Message);
	}

	private async Task LoginAsync()
	{
		var loginRequest = new LoginRequest
		{
			Email = "test3@test.com",
			Password = "Passw0rd@"
		};

		var loginResponse = await _accountClient.LoginAsync(loginRequest);
		Assert.Equal(200, (int)loginResponse!.StatusCode);
	}
}
