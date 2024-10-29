using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Api.Endpoints;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "OK" });
app.MapEndpoints();
app.MapGroup("v1/identity")
	.WithTags("Identity")
	.MapIdentityApi<User>();

app.MapGroup("v1/identity")
	.WithTags("Identity")
	.MapPost("/logout", async (SignInManager<User> signInManager) =>
	{
		await signInManager.SignOutAsync();
		return Results.Ok();
	})
	.RequireAuthorization();

app.MapGroup("v1/identity")
	.WithTags("Identity")
	.MapPost("/roles", (ClaimsPrincipal user) =>
	{
		if (user.Identity is null || !user.Identity.IsAuthenticated)
			return Results.Unauthorized();

		var identity = (ClaimsIdentity)user.Identity;
		var roles = identity.FindAll(identity.RoleClaimType).Select(c => new
		{
			c.Issuer,
			c.OriginalIssuer,
			c.Type,
			c.Value,
			c.ValueType
		});

		return TypedResults.Json(roles);
	})
	.RequireAuthorization();

app.Run();
