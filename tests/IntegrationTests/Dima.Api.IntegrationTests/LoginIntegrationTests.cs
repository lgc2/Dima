using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Fixtures;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests;

public class LoginIntegrationTests : IClassFixture<LoginTestFixture>
{
	private readonly LoginTestFixture _fixture;
	private readonly AccountClient _accountClient;

	public LoginIntegrationTests(LoginTestFixture fixture)
	{
		_fixture = fixture;
		_accountClient = new AccountClient(fixture.HttpClient);
	}

	[Fact]
	public async Task ShouldLoginSuccessfully()
	{
		var loginRequest = new LoginRequest
		{
			Email = _fixture.RegisterRequest.Email,
			Password = _fixture.RegisterRequest.Password
		};

		var loginResponse = await _accountClient.LoginAsync(loginRequest);
		Assert.Equal(200, (int)loginResponse!.StatusCode);
	}

	[Fact]
	public async Task ShouldNotLogin_WhenPasswordIsIncorrect()
	{
		var loginRequest = new LoginRequest
		{
			Email = _fixture.RegisterRequest.Email,
			Password = "passw0rd@3"
		};

		var loginResponse = await _accountClient.LoginAsync(loginRequest);
		Assert.Equal(401, (int)loginResponse!.StatusCode);
	}
}
