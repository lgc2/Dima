using System.Net;
using System.Net.Http.Json;

namespace Dima.Api.IntegrationTests.Clients;

public class ApiClientBase
{
	protected readonly HttpClient _httpClient;
	private readonly CookieContainer _cookieContainer;

	public ApiClientBase(IHttpClientFactory httpClientFactory)
	{
		_cookieContainer = new CookieContainer();
		var handler = new HttpClientHandler
		{
			CookieContainer = _cookieContainer,
			UseCookies = true
		};

		_httpClient = new HttpClient(handler);
		_httpClient = httpClientFactory.CreateClient("dima");
	}

	public async Task<T?> GetAsync<T>(string url)
	{
		var response = await _httpClient.GetAsync(url);
		return await response.Content.ReadFromJsonAsync<T>();
	}

	public async Task<TResult?> PostAsync<TData, TResult>(string url, TData data)
	{
		var response = await _httpClient.PostAsJsonAsync(url, data);
		return await response.Content.ReadFromJsonAsync<TResult>();
	}

	public async Task<T?> UpdateAsync<T>(string url, T data)
	{
		var response = await _httpClient.PutAsJsonAsync(url, data);
		return await response.Content.ReadFromJsonAsync<T>();
	}
}
