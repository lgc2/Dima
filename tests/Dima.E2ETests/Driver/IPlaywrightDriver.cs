using Microsoft.Playwright;

namespace Dima.E2ETests.Driver
{
	public interface IPlaywrightDriver
	{
		Task<IBrowser> Browser { get; }
		Task<IBrowserContext> BrowserContext { get; }
		Task<IPage> Page { get; }
		Task<IAPIRequestContext> ApiRequestContext { get; }
		Task<string> TakeScreenshotAsPathAsync(string fileName);
		Task DisposeAsync();
	}
}