using Dima.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x =>
{
	x.UseSqlServer(cnnStr);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
	x.CustomSchemaIds(n => n.FullName);
});
builder.Services.AddTransient<Handler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
	"/v1/transactions",
	(Request request, Handler handler) => handler.Handle(request))
	.WithName("Transactions: Create")
	.WithSummary("Create a new transaction")
	.Produces<Response>();

app.Run();

public class Request
{
	public string Title { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public int Type { get; set; }
	public decimal Amount { get; set; }
	public long CategoryId { get; set; }
	public string UserId { get; set; } = string.Empty;
}

public class Response
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
}

public class Handler
{
	public Response Handle(Request request)
	{
		// processes the creation...
		// persists in the database...
		return new Response
		{
			Id = 4,
			Title = request.Title
		};
	}
}
