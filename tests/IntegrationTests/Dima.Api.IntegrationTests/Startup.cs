﻿using Microsoft.Extensions.DependencyInjection;

namespace Dima.Api.IntegrationTests;

public class Startup
{
	public static ServiceProvider ServiceProvider { get; private set; }

	public static void ConfigureServices()
	{
		var serviceCollection = new ServiceCollection();
		serviceCollection.AddHttpClient("dima", client =>
		{
			client.BaseAddress = new Uri(Configuration.BackendUrl);
		});
		ServiceProvider = serviceCollection.BuildServiceProvider();
	}
}
