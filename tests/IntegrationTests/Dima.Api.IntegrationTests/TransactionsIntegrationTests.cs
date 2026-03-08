using Dima.Api.IntegrationTests.Fixtures;

namespace Dima.Api.IntegrationTests;

[Collection("SeededCollection")]
public class TransactionsIntegrationTests
{
    private readonly SeedDataTestFixture _fixture;

    public TransactionsIntegrationTests(SeedDataTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdEndpoint_ShouldReturnSuccess()
    {
        var getByIdResponse = await _fixture.TransactionsClient.GetByIdAsync(_fixture.Transaction!.Data!.Id);
        Assert.Equal(200, getByIdResponse!.Code);
        Assert.Equal(_fixture.CreateTransactionReq.Title, getByIdResponse!.Data!.Title);
        Assert.Equal(_fixture.CreateTransactionReq.Type, getByIdResponse!.Data!.Type);
        Assert.Equal(_fixture.CreateTransactionReq.Amount, getByIdResponse!.Data!.Amount);
        Assert.Equal(_fixture.CreateTransactionReq.CategoryId, getByIdResponse!.Data!.CategoryId);
        Assert.Equal(_fixture.CreateTransactionReq.PaidOrReceivedAt, getByIdResponse!.Data!.PaidOrReceivedAt);
        Assert.Equal(_fixture.CreateTransactionReq.UserId, getByIdResponse!.Data!.UserId);
        Assert.Null(getByIdResponse.Message);
    }
}
