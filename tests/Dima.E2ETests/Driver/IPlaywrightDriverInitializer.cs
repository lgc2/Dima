using Dima.E2ETests.Config;
using Microsoft.Playwright;

namespace Dima.E2ETests.Driver
{
	public interface IPlaywrightDriverInitializer
	{
		Task<IBrowser> GetChromeDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetChromiumDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetEdgeDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetFirefoxDriverAsync(TestSettings testSettings);
		Task<IBrowser> GetWebkitDriverAsync(TestSettings testSettings);
	}
}