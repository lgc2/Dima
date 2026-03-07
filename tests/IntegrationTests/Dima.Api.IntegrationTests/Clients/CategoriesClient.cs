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

    public async Task<Response<Category>?> CreateAsync(CreateCategoryRequest data) => await PostAsync<CreateCategoryRequest, Response<Category>?>($"/v1/categories", data);

    public async Task<Response<Category>?> DeleteAsync(long id) => await DeleteAsync<Response<Category>?>($"/v1/categories/{id}");
}
