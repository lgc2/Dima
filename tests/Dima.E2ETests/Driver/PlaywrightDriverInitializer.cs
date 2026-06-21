using Dima.E2ETests.Config;
using Microsoft.Playwright;

namespace Dima.E2ETests.Driver;

public class PlaywrightDriverInitializer : IPlaywrightDriverInitializer
{
	public async Task<IBrowser> GetChromeDriverAsync(TestSettings testSettings)
	{
		var options = GetParameters(testSettings);
		options.Channel = "chrome";
		return await GetBrowserAsync(DriverType.Chromium, options);
	}

	public async Task<IBrowser> GetChromiumDriverAsync(TestSettings testSettings)
	{
		var options = GetParameters(testSettings);
		options.Channel = "chromium";
		return await GetBrowserAsync(DriverType.Chromium, options);
	}

	public async Task<IBrowser> GetEdgeDriverAsync(TestSettings testSettings)
	{
		var options = GetParameters(testSettings);
		options.Channel = "msedge";
		return await GetBrowserAsync(DriverType.Chromium, options);
	}

	public async Task<IBrowser> GetFirefoxDriverAsync(TestSettings testSettings)
	{
		var options = GetParameters(testSettings);
		options.Channel = "firefox";
		return await GetBrowserAsync(DriverType.Firefox, options);
	}

	public async Task<IBrowser> GetWebkitDriverAsync(TestSettings testSettings)
	{
		var options = GetParameters(testSettings);
		options.Channel = "";
		return await GetBrowserAsync(DriverType.Webkit, options);
	}

	private async Task<IBrowser> GetBrowserAsync(DriverType driverType, BrowserTypeLaunchOptions options)
	{
		var playwright = await Playwright.CreateAsync();
		return await playwright[driverType.ToString().ToLower()].LaunchAsync(options);
	}

	private BrowserTypeLaunchOptions GetParameters(TestSettings testSettings)
	{
		return new BrowserTypeLaunchOptions
		{
			Args = testSettings.Args,
			Headless = testSettings.Headless,
			Timeout = ToMilliseconds(testSettings.Timeout),
			SlowMo = testSettings.SlowMo
		};
	}

	private float? ToMilliseconds(float? seconds)
	{
		return seconds * 1000;
	}
}
