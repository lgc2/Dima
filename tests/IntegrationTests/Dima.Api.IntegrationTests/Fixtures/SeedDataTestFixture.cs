using Dima.Core.Enums;
using Dima.Core.Models;
using Dima.Core.Requests.Account;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.IntegrationTests.Fixtures;

public class SeedDataTestFixture : TestFixtureBase
{
    public CreateCategoryRequest CreateCategoryReq { get; private set; } = null!;
    public CreateTransactionRequest CreateTransactionReq { get; private set; } = null!;
    public List<Response<Category>?> Categories { get; set; } = [];
    public long SeededCategoryId { get; private set; } = 0;
    public Response<Transaction>? Transaction { get; private set; }

    public override async Task InitializeAsync()
    {
        await AccountClient.RegisterAsync(RegisterRequest);
        await Login();
        await SeedData();
    }

    public override async Task DisposeAsync()
    {
        foreach (var category in Categories)
        {
            await CategoriesClient.DeleteAsync(category!.Data!.Id);    
        }

        await TransactionsClient.DeleteAsync(Transaction!.Data!.Id);
        await _factory.DisposeAsync();
    }

    private async Task Login()
    {
        var loginRequest = new LoginRequest
        {
            Email = RegisterRequest.Email,
            Password = RegisterRequest.Password
        };

        var loginResponse = await AccountClient.LoginAsync(loginRequest);
        Assert.Equal(200, (int)loginResponse!.StatusCode);
    }

    private async Task SeedData()
    {
        CreateCategoryReq = new CreateCategoryRequest
        {
            UserId = RegisterRequest.Email,
            Title = $"Tech Learning {new Random().Next(0, 1000000)}",
            Description = $"Learning expanses {new Random().Next(0, 1000000)}"
        };
        var seededCategory = await CategoriesClient.CreateAsync(CreateCategoryReq);
        SeededCategoryId = seededCategory!.Data!.Id;
        Categories.Add(seededCategory);

        CreateTransactionReq = new CreateTransactionRequest
        {
            UserId = RegisterRequest.Email,
            Title = $"Transaction Title {new Random().Next(0, 1000000)}",
            Type = ETransactionType.Deposit,
            Amount = 157.63m,
            CategoryId = SeededCategoryId,
            PaidOrReceivedAt =  DateTime.UtcNow,
        };
        Transaction = await TransactionsClient.CreateAsync(CreateTransactionReq);
    }
}
