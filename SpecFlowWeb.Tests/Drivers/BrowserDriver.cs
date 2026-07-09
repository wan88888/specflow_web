using PuppeteerSharp;
using PuppeteerSharp.BrowserData;

namespace SpecFlowWeb.Tests.Drivers;

/// <summary>
/// Manages the PuppeteerSharp browser lifecycle for UI tests.
/// </summary>
public sealed class BrowserDriver : IAsyncDisposable
{
    private static readonly string BrowserCachePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".cache",
        "specflow_web",
        "chromium");

    private static readonly SemaphoreSlim DownloadLock = new(1, 1);
    private static string? _headlessExecutablePath;

    private IBrowser? _browser;
    private IPage? _page;

    public IPage Page =>
        _page ?? throw new InvalidOperationException("Browser has not been started. Call StartAsync first.");

    public async Task StartAsync(bool headless = true)
    {
        var launchOptions = headless
            ? CreateHeadlessLaunchOptions(await EnsureHeadlessBrowserReadyAsync())
            : CreateHeadedLaunchOptions();

        _browser = await Puppeteer.LaunchAsync(launchOptions);

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

    private static LaunchOptions CreateHeadlessLaunchOptions(string executablePath) =>
        new()
        {
            Headless = true,
            HeadlessMode = HeadlessMode.True,
            ExecutablePath = executablePath,
            Args = ["--no-sandbox", "--disable-setuid-sandbox"],
            DefaultViewport = new ViewPortOptions { Width = 1280, Height = 720 }
        };

    private static LaunchOptions CreateHeadedLaunchOptions() =>
        new()
        {
            Headless = false,
            HeadlessMode = HeadlessMode.False,
            Channel = ChromeReleaseChannel.Stable,
            Args =
            [
                "--start-maximized",
                "--disable-blink-features=AutomationControlled"
            ],
            DefaultViewport = null
        };

    private static async Task<string> EnsureHeadlessBrowserReadyAsync()
    {
        if (_headlessExecutablePath is not null)
        {
            return _headlessExecutablePath;
        }

        await DownloadLock.WaitAsync();
        try
        {
            if (_headlessExecutablePath is not null)
            {
                return _headlessExecutablePath;
            }

            var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Path = BrowserCachePath,
                Browser = SupportedBrowser.Chrome
            });

            var installedBrowsers = browserFetcher.GetInstalledBrowsers();
            if (!installedBrowsers.Any())
            {
                Console.WriteLine("Downloading Chromium for headless UI tests (first run only)...");
                await browserFetcher.DownloadAsync();
                installedBrowsers = browserFetcher.GetInstalledBrowsers();
            }

            _headlessExecutablePath = installedBrowsers.First().GetExecutablePath();
            return _headlessExecutablePath;
        }
        finally
        {
            DownloadLock.Release();
        }
    }
}
