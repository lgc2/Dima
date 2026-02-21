using System.Net.Http.Json;
using Dima.Core.Requests.Account;

namespace Dima.Api.IntegrationTests.Clients;

public class AccountClient : ApiClientBase
{
	public AccountClient(HttpClient httpClient) : base(httpClient)
	{
	}

	public async Task<HttpResponseMessage?> RegisterAsync(RegisterRequest request) =>
		await _httpClient.PostAsJsonAsync("/v1/identity/register", request);

	public async Task<HttpResponseMessage?> LoginAsync(LoginRequest request) =>
		await _httpClient.PostAsJsonAsync($"/v1/identity/login?useCookies=true", request);
}
