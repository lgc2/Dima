using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Factory;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Fixtures;

public class TestFixture : IAsyncLifetime
{
    private readonly TestWebApplicationFactory _factory = new();
    private readonly AccountClient _accountClient;
    public HttpClient HttpClient { get; private set; } = null!;

    public TestFixture()
    {
        HttpClient = _factory.CreateClient();
        _accountClient = new AccountClient(HttpClient);
    }

    public async Task InitializeAsync()
    {
        var loginRequest = new LoginRequest
        {
            Email = "test3@test.com",
            Password = "Passw0rd@"
        };

        var loginResponse = await _accountClient.LoginAsync(loginRequest);
        Assert.Equal(200, (int)loginResponse!.StatusCode);
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}
