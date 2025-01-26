using System.Net.Http.Json;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Clients;

public class AccountClient : ApiClientBase
{
	public AccountClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
	{
	}
	public async Task<HttpResponseMessage?> LoginAsync(LoginRequest request) =>
		await _httpClient.PostAsJsonAsync($"{Configuration.BackendUrl}/v1/identity/login?useCookies=true", request);
}
