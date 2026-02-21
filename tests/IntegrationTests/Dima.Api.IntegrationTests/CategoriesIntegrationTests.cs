using Dima.Api.IntegrationTests.Clients;
using Dima.Api.IntegrationTests.Fixtures;

namespace Dima.Api.IntegrationTests;

public class CategoriesIntegrationTests : IClassFixture<AuthenticatedTestFixture>
{
	private readonly CategoriesClient _categoriesClient;

	public CategoriesIntegrationTests(AuthenticatedTestFixture fixture)
	{
		_categoriesClient = new CategoriesClient(fixture.HttpClient);
	}

	[Fact]
	public async Task GetByIdEndpoint_ReturnsSuccess()
	{
		var getByIdResponse = await _categoriesClient.GetByIdAsync(10020);
		Assert.Equal(200, getByIdResponse!._code);
		Assert.Equal(10020, getByIdResponse!.Data!.Id);
		Assert.Equal("Tech Lerning (title changed 1)", getByIdResponse!.Data!.Title);
		Assert.Equal("Learning expanses (description changed 3)", getByIdResponse!.Data!.Description);
		Assert.Equal("test3@test.com", getByIdResponse!.Data!.UserId);
		Assert.Null(getByIdResponse.Message);
	}
}
