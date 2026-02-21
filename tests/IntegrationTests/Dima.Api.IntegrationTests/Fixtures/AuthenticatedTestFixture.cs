using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Fixtures;

public class AuthenticatedTestFixture : TestFixtureBase
{
    public override async Task InitializeAsync()
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
