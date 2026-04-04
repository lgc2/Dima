using Dima.Api.IntegrationTests.Fixtures;
using Dima.Core.Requests.Categories;

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
		var getByIdResponse = await _fixture.CategoriesClient.GetByIdAsync(_fixture.SeededCategoryId);
		Assert.Equal(200, getByIdResponse!.Code);
		Assert.Equal(_fixture.CreateCategoryReq.Title, getByIdResponse.Data!.Title);
		Assert.Equal(_fixture.CreateCategoryReq.Description, getByIdResponse.Data!.Description);
		Assert.Equal(_fixture.CreateCategoryReq.UserId, getByIdResponse.Data!.UserId);
		Assert.Null(getByIdResponse.Message);
	}

	[Fact]
	public async Task CreateEndpoint_ShouldReturnSuccess()
	{
		var createRequest = new CreateCategoryRequest
		{
			UserId = _fixture.RegisterRequest.Email,
			Title = $"Tech Learning 2 {new Random().Next(0, 1000000)}",
			Description = $"Learning expanses 2 {new Random().Next(0, 1000000)}"
		};

		var createResponse = await _fixture.CategoriesClient.CreateAsync(createRequest);
		_fixture.Categories.Add(createResponse);

		Assert.Equal(200, createResponse!.Code);
		Assert.Equal(createRequest.Title, createResponse.Data!.Title);
		Assert.Equal(createRequest.Description, createResponse.Data!.Description);
		Assert.Equal(createRequest.UserId, createResponse.Data!.UserId);
		Assert.Equal("Category created", createResponse.Message);
	}

	[Fact]
	public async Task DeleteEndpoint_ShouldReturnSuccess()
	{
		var createRequest = new CreateCategoryRequest
		{
			UserId = _fixture.RegisterRequest.Email,
			Title = $"Tech Learning - To be deleted {new Random().Next(0, 1000000)}",
			Description = $"Learning expanses - To be deleted {new Random().Next(0, 1000000)}"
		};

		var createResponse = await _fixture.CategoriesClient.CreateAsync(createRequest);

		Assert.Equal(200, createResponse!.Code);
		Assert.Equal(createRequest.Title, createResponse.Data!.Title);
		Assert.Equal(createRequest.Description, createResponse.Data!.Description);
		Assert.Equal(createRequest.UserId, createResponse.Data!.UserId);
		Assert.Equal("Category created", createResponse.Message);

		var deleteResponse = await _fixture.CategoriesClient.DeleteAsync(createResponse.Data.Id);

		Assert.Equal(200, deleteResponse!.Code);
		Assert.Equal(createRequest.Title, deleteResponse.Data!.Title);
		Assert.Equal(createRequest.Description, deleteResponse.Data!.Description);
		Assert.Equal(createRequest.UserId, deleteResponse.Data!.UserId);
		Assert.Equal("Category deleted", deleteResponse.Message);
	}

	[Fact]
	public async Task GetAllEndpoint_ShouldReturnSuccess()
	{
		var createRequest = new CreateCategoryRequest
		{
			UserId = _fixture.RegisterRequest.Email,
			Title = $"Tech Learning - 3 {new Random().Next(0, 1000000)}",
			Description = $"Learning expanses - 3 {new Random().Next(0, 1000000)}"
		};

		var createResponse = await _fixture.CategoriesClient.CreateAsync(createRequest);
		_fixture.Categories.Add(createResponse);

		Assert.Equal(200, createResponse!.Code);
		Assert.Equal(createRequest.Title, createResponse.Data!.Title);
		Assert.Equal(createRequest.Description, createResponse.Data!.Description);
		Assert.Equal(createRequest.UserId, createResponse.Data!.UserId);
		Assert.Equal("Category created", createResponse.Message);

		var getAllResponse = await _fixture.CategoriesClient.GetAllAsync();

		Assert.Equal(200, getAllResponse!.Code);
		Assert.True(getAllResponse.Data!.Count >= 2);
		Assert.Contains(_fixture.SeededCategoryId, getAllResponse.Data.Select(x => x.Id));
		Assert.Contains(createResponse.Data.Id, getAllResponse.Data.Select(x => x.Id));
		Assert.Null(getAllResponse.Message);
		Assert.Equal(1, getAllResponse.CurrentPage);
		Assert.Equal(1, getAllResponse.TotalPages);
		Assert.Equal(250, getAllResponse.PageSize);
		Assert.True(getAllResponse.TotalCount >= 2);
	}

	[Fact]
	public async Task UpdateEndpoint_ShouldReturnSuccess()
	{
		var createRequest = new CreateCategoryRequest
		{
			UserId = _fixture.RegisterRequest.Email,
			Title = $"Tech Learning - To be edited {new Random().Next(0, 1000000)}",
			Description = $"Learning expanses - To be edited {new Random().Next(0, 1000000)}"
		};

		var createResponse = await _fixture.CategoriesClient.CreateAsync(createRequest);
		_fixture.Categories.Add(createResponse);

		Assert.Equal(200, createResponse!.Code);
		Assert.Equal(createRequest.Title, createResponse.Data!.Title);
		Assert.Equal(createRequest.Description, createResponse.Data!.Description);
		Assert.Equal(createRequest.UserId, createResponse.Data!.UserId);
		Assert.Equal("Category created", createResponse.Message);

		var updateRequest = new UpdateCategoryRequest()
		{
			Id = createResponse.Data.Id,
			UserId = _fixture.RegisterRequest.Email,
			Title = "Category updated",
			Description = "Category updated",
		};

		var updateResponse = await _fixture.CategoriesClient.UpdateAsync(updateRequest);

		Assert.Equal(200, updateResponse!.Code);
		Assert.Equal(createResponse.Data.Id, updateResponse.Data!.Id);
		Assert.Equal(updateRequest.Title, updateResponse.Data!.Title);
		Assert.Equal(updateRequest.Description, updateResponse.Data!.Description);
		Assert.Equal(updateRequest.UserId, updateResponse.Data!.UserId);
		Assert.Equal("Category updated", updateResponse.Message);
	}
}
