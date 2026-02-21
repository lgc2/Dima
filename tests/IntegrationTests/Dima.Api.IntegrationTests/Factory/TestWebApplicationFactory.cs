using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dima.Api.IntegrationTests.Factory;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			// You can add test-specific service overrides here if needed
			// For example, you might want to use an in-memory database instead of the real one
		});

		builder.UseEnvironment("Development");
	}
}
