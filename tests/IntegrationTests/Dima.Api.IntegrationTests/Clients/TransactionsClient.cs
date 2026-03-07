using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.IntegrationTests.Clients;

public class TransactionsClient : ApiClientBase
{
    public TransactionsClient(HttpClient httpCient) : base(httpCient)
    {
    }

    public async Task<Response<Transaction>?> GetByIdAsync(long id) => await GetAsync<Response<Transaction>?>($"/v1/transactions/{id}");

    public async Task<Response<Transaction>?> CreateAsync(CreateTransactionRequest data) => await PostAsync<CreateTransactionRequest, Response<Transaction>?>($"/v1/transactions", data);

    public async Task<Response<Transaction>?> DeleteAsync(long id) => await DeleteAsync<Response<Transaction>?>($"/v1/transactions/{id}");
}
