using System.Net.Http.Json;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
	private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);

	public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
	{
		var result = await _client.PostAsJsonAsync("v1/transactions", request);
		var content = await result.Content.ReadFromJsonAsync<Response<Transaction?>>();

		if (content is null)
			return new Response<Transaction?>(null, 400, "Falha ao criar transação");

		return new Response<Transaction?>(content.Data, (int)result.StatusCode, content.Message);
	}

	public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
	{
		var result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
		var content = await result.Content.ReadFromJsonAsync<Response<Transaction?>>();

		if (content is null)
			return new Response<Transaction?>(null, 400, "Falha ao atualizar a transação");

		return new Response<Transaction?>(content.Data, (int)result.StatusCode, content.Message);
	}

	public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
	{
		var result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
		var content = await result.Content.ReadFromJsonAsync<Response<Transaction?>>();

		if (content is null)
			return new Response<Transaction?>(null, 400, "Falha ao excluir a transação");

		return new Response<Transaction?>(content.Data, (int)result.StatusCode, content.Message);
	}

	public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request) =>
		await _client.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
		?? new Response<Transaction?>(null, 400, $"Falha ao tentar retornar a transação {request.Id}");

	public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransationByPeriodRequest request)
	{
		const string format = "yyyy-MM-dd";
		var startDate = request.StartDate is not null ? request.StartDate.Value.ToString(format) : DateTime.UtcNow.GetFirstDay().ToString(format);
		var endDate = request.EndDate is not null ? request.EndDate.Value.ToString(format) : DateTime.UtcNow.GetLastDay().ToString(format);

		var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

		return await _client.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url)
		?? new PagedResponse<List<Transaction>?>(null, 400, "Falha ao tentar retornar as transações");
	}
}
