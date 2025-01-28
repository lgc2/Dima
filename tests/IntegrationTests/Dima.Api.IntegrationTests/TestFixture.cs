using Dima.Api.IntegrationTests.Clients;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests;

public class TestFixture : IDisposable
{
	private readonly AccountClient _accountClient;

	public TestFixture()
	{
		_accountClient = new AccountClient();
		LoginAsync().Wait();
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

	public void Dispose()
	{
	}
}
