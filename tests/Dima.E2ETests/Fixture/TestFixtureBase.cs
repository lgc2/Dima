using Dima.E2ETests.Config;
using Dima.E2ETests.Driver;
using Microsoft.Playwright;

namespace Dima.E2ETests.Fixture;

public class TestFixtureBase : IAsyncLifetime
{
    public TestFixtureBase()
    {
        TestSettings = ConfigReader.ReadConfig();
        PlaywrightDriver = new PlaywrightDriver(TestSettings, new PlaywrightDriverInitializer());
        Page = PlaywrightDriver.Page;
    }

    public TestSettings TestSettings;
    public IPlaywrightDriver PlaywrightDriver;
    public Task<IPage> Page;

    public async Task NavigateToUrl()
    {
        await (await Page).GotoAsync(TestSettings.ApplicationUrl);
    }

    public async Task TakeScreenshotAsync(string filename)
    {
        await (await Page).ScreenshotAsync(new PageScreenshotOptions() { Path = filename, FullPage = true });
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await PlaywrightDriver.DisposeAsync();
    }
}
