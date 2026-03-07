using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Fixtures;

namespace Dima.Api.IntegrationTests;

public class CategoriesIntegrationTests : IClassFixture<SeedDataTestFixture>
{
	private readonly SeedDataTestFixture _fixture;
	private readonly CategoriesClient _categoriesClient;

	public CategoriesIntegrationTests(SeedDataTestFixture fixture)
	{
		_fixture = fixture;
		_categoriesClient = new CategoriesClient(fixture.HttpClient);
	}

	[Fact]
	public async Task GetByIdEndpoint_ShouldReturnSuccess()
	{
		var getByIdResponse = await _categoriesClient.GetByIdAsync(_fixture.Category!.Data!.Id);
		Assert.Equal(200, getByIdResponse!.Code);
		Assert.Equal(_fixture.CreateCategoryReq.Title, getByIdResponse!.Data!.Title);
		Assert.Equal(_fixture.CreateCategoryReq.Description, getByIdResponse!.Data!.Description);
		Assert.Equal(_fixture.CreateCategoryReq.UserId, getByIdResponse!.Data!.UserId);
		Assert.Null(getByIdResponse.Message);
	}
}
