using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Factory;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Fixtures;

public class TestFixtureBase : IAsyncLifetime
{
    protected readonly TestWebApplicationFactory _factory = new();
    public readonly AccountClient AccountClient;
    public readonly CategoriesClient CategoriesClient;
    public readonly TransactionsClient TransactionsClient;
    private HttpClient _httpClient { get; } = null!;

    public readonly RegisterRequest RegisterRequest = new()
    {
        Email = $"test{new Random().Next(0, 1000000)}@test.com",
        Password = "Passw0rd@3"
    };

    public TestFixtureBase()
    {
        _httpClient = _factory.CreateClient();
        AccountClient = new AccountClient(_httpClient);
        CategoriesClient = new CategoriesClient(_httpClient);
        TransactionsClient = new TransactionsClient(_httpClient);
    }

    public virtual async Task InitializeAsync()
    {
        await AccountClient.RegisterAsync(RegisterRequest);
    }

    public virtual async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}
