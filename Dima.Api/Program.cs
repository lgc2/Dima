using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/v1/categories", async (CreateCategoryRequest request, ICategoryHandler handler) =>
{
	var response = await handler.CreateAsync(request);
	return Results.Json(response, statusCode: response._code);
})
	.WithName("Categories: Create")
	.WithSummary("Create a new category")
	.Produces<Response<Category?>>();

app.MapPut("/v1/categories/{id}", async (long id, UpdateCategoryRequest request, ICategoryHandler handler) =>
{
	request.Id = id;
	var response = await handler.UpdateAsync(request);
	return Results.Json(response, statusCode: response._code);
})
	.WithName("Categories: Update")
	.WithSummary("Update a category")
	.Produces<Response<Category?>>();

app.MapDelete("/v1/categories/{id}", async (long id, [FromBody] DeleteCategoryRequest request, ICategoryHandler handler) =>
{
	request.Id = id;
	var response = await handler.DeleteAsync(request);
	return Results.Json(response, statusCode: response._code);
})
	.WithName("Categories: Delete")
	.WithSummary("Delete a category")
	.Produces<Response<Category?>>();

app.Run();
