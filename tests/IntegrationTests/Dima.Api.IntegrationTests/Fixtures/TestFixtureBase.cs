using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Factory;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Fixtures;

public class TestFixtureBase : IAsyncLifetime
{
    protected readonly TestWebApplicationFactory _factory = new();
    protected readonly AccountClient _accountClient;
    protected readonly CategoriesClient _categoriesClient;
    public HttpClient HttpClient { get; private set; } = null!;

    public readonly RegisterRequest RegisterRequest = new()
    {
        Email = $"test{new Random().Next(0, 1000000)}@test.com",
        Password = "Passw0rd@3"
    };

    public TestFixtureBase()
    {
        HttpClient = _factory.CreateClient();
        _accountClient = new AccountClient(HttpClient);
        _categoriesClient = new CategoriesClient(HttpClient);
    }

    public virtual async Task InitializeAsync()
    {
        await _accountClient.RegisterAsync(RegisterRequest);
    }

    public virtual async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}
