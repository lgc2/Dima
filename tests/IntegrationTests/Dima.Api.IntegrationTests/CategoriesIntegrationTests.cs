using Dima.Api.IntegrationTests.Fixtures;

namespace Dima.Api.IntegrationTests;

[Collection("SeededCollection")]
public class CategoriesIntegrationTests
{
	private readonly SeedDataTestFixture _fixture;

	public CategoriesIntegrationTests(SeedDataTestFixture fixture)
	{
		_fixture = fixture;
	}

	[Fact]
	public async Task GetByIdEndpoint_ShouldReturnSuccess()
	{
		var getByIdResponse = await _fixture.CategoriesClient.GetByIdAsync(_fixture.Category!.Data!.Id);
		Assert.Equal(200, getByIdResponse!.Code);
		Assert.Equal(_fixture.CreateCategoryReq.Title, getByIdResponse!.Data!.Title);
		Assert.Equal(_fixture.CreateCategoryReq.Description, getByIdResponse!.Data!.Description);
		Assert.Equal(_fixture.CreateCategoryReq.UserId, getByIdResponse!.Data!.UserId);
		Assert.Null(getByIdResponse.Message);
	}
}
