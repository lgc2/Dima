using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.IntegrationTests.Clients;

public class CategoriesClient : ApiClientBase
{
    public CategoriesClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<Response<Category>?> GetByIdAsync(long id) => await GetAsync<Response<Category>?>($"/v1/categories/{id}");

    public async Task<Response<Category>?> CreateAsync(CreateCategoryRequest data) => await PostAsync<CreateCategoryRequest, Response<Category>?>("/v1/categories", data);

    public async Task<Response<Category>?> DeleteAsync(long id) => await DeleteAsync<Response<Category>?>($"/v1/categories/{id}");

    public async Task<PagedResponse<List<Category>?>?> GetAllAsync() => await GetAsync<PagedResponse<List<Category>?>>("/v1/categories?pageNumber=1&pageSize=250");

    public async Task<Response<Category>?> UpdateAsync(UpdateCategoryRequest data) => await UpdateAsync<UpdateCategoryRequest, Response<Category>?>($"/v1/categories/{data.Id}", data);
}
