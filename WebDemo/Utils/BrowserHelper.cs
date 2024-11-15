using PuppeteerSharp;

namespace WebDemo.Utils
{
    public class BrowserHelper
    {
        protected static IBrowser _browser;
        protected static IPage _page;
        protected async Task Init()
        {
            var revision = await new BrowserFetcher(SupportedBrowser.Chromium).DownloadAsync();

            _browser = await Puppeteer.LaunchAsync(new LaunchOptions()
            {
                Headless = false,
                ExecutablePath = revision.GetExecutablePath()
            });

            _page = await _browser.NewPageAsync();
        }
    }
}