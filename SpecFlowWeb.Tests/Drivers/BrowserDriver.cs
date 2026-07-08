using PuppeteerSharp;

namespace SpecFlowWeb.Tests.Drivers;

/// <summary>
/// Manages the PuppeteerSharp browser lifecycle for UI tests.
/// </summary>
public sealed class BrowserDriver : IAsyncDisposable
{
    private IBrowser? _browser;
    private IPage? _page;

    public IPage Page =>
        _page ?? throw new InvalidOperationException("Browser has not been started. Call StartAsync first.");

    public async Task StartAsync(bool headless = true)
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        var launchArgs = new List<string> { "--no-sandbox", "--disable-setuid-sandbox" };
        if (!headless)
        {
            launchArgs.Add("--start-maximized");
        }

        _browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = headless,
            Args = [..launchArgs],
            DefaultViewport = headless
                ? new ViewPortOptions { Width = 1280, Height = 720 }
                : null
        });

        var pages = await _browser.PagesAsync();
        _page = pages.Length > 0 ? pages[0] : await _browser.NewPageAsync();

        if (headless)
        {
            await _page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1280,
                Height = 720
            });
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_page is not null)
        {
            await _page.CloseAsync();
            _page = null;
        }

        if (_browser is not null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }
    }
}
