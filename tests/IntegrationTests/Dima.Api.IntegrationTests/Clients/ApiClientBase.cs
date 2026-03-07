using System.Net.Http.Json;

namespace Dima.Api.IntegrationTests.Clients;

public class ApiClientBase
{
    protected readonly HttpClient _httpClient;

    protected ApiClientBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<TResult?> PostAsync<TData, TResult>(string url, TData data)
    {
        var response = await _httpClient.PostAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<TResult>();
    }

    protected async Task<TResult?> UpdateAsync<TData, TResult>(string url, TData data)
    {
        var response = await _httpClient.PutAsJsonAsync(url, data);
        return await response.Content.ReadFromJsonAsync<TResult>();
    }

    protected async Task<T?> DeleteAsync<T>(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        return await response.Content.ReadFromJsonAsync<T>();
    }
}
