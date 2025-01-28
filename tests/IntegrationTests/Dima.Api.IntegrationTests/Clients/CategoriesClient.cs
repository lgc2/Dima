using Dima.Core.Models;
using Dima.Core.Responses;

namespace Dima.Api.IntegrationTests.Clients;

public class CategoriesClient : ApiClientBase
{
	public async Task<Response<Category>?> GetByIdAsync(long id) =>
		await GetAsync<Response<Category>?>($"{Configuration.BackendUrl}/v1/categories/{id}");
}
