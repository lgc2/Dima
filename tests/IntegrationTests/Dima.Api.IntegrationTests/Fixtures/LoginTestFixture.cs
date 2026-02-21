using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Fixtures;

public class LoginTestFixture : TestFixtureBase
{
	public RegisterRequest RegisterRequest = new()
	{
		Email = $"test{new Random().Next(0, 1000000)}@test.com",
		Password = "Passw0rd@3"
	};

	public override async Task InitializeAsync()
	{
		await _accountClient.RegisterAsync(RegisterRequest);
	}
}
