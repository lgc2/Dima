﻿using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
	private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

	public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
	{
		var result = await _client.PostAsJsonAsync("v1/categories", request);
		var content = await result.Content.ReadFromJsonAsync<Response<Category?>>();

		if (content is null)
			return new Response<Category?>(null, 400, "Falha ao criar categoria");

		return new Response<Category?>(content.Data, (int)result.StatusCode, content.Message);
	}

	public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
	{
		var result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
		var content = await result.Content.ReadFromJsonAsync<Response<Category?>>();

		if (content is null)
			return new Response<Category?>(null, 400, "Falha ao atualizar a categoria");

		return new Response<Category?>(content.Data, (int)result.StatusCode, content.Message);
	}

	public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
	{
		var result = await _client.DeleteAsync($"v1/categories/{request.Id}");
		var content = await result.Content.ReadFromJsonAsync<Response<Category?>>();

		if (content is null)
			return new Response<Category?>(null, 400, "Falha ao excluir a categoria");

		return new Response<Category?>(content.Data, (int)result.StatusCode, content.Message);
	}

	public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request) =>
		await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}")
		?? new Response<Category?>(null, 400, $"Falha ao tentar retornar a categoria {request.Id}");

	public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request) =>
		await _client.GetFromJsonAsync<PagedResponse<List<Category>?>>("v1/categories")
		?? new PagedResponse<List<Category>?>(null, 400, "Falha ao tentar retornar as categorias");
}
