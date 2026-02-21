using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Factory;

namespace Dima.Api.IntegrationTests.Fixtures;

public class TestFixtureBase : IAsyncLifetime
{
    protected readonly TestWebApplicationFactory _factory = new();
    protected readonly AccountClient _accountClient;
    public HttpClient HttpClient { get; private set; } = null!;

    public TestFixtureBase()
    {
        HttpClient = _factory.CreateClient();
        _accountClient = new AccountClient(HttpClient);
    }

    public virtual async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}
