using PuppeteerSharp;

namespace SpecFlowWeb.Tests.Pages;

public sealed class InventoryPage
{
    private const string TitleSelector = ".title";
    private const string InventoryContainerSelector = "#inventory_container";

    private readonly IPage _page;

    public InventoryPage(IPage page)
    {
        _page = page;
    }

    public async Task WaitUntilLoadedAsync()
    {
        await _page.WaitForSelectorAsync(InventoryContainerSelector);
        await _page.WaitForSelectorAsync(TitleSelector);
    }

    public async Task<string> GetTitleAsync()
    {
        await WaitUntilLoadedAsync();
        return await _page.EvaluateExpressionAsync<string>(
            "document.querySelector('.title')?.textContent?.trim() ?? ''");
    }

    public async Task<bool> IsDisplayedAsync()
    {
        try
        {
            await WaitUntilLoadedAsync();
            return _page.Url.Contains("inventory.html", StringComparison.OrdinalIgnoreCase);
        }
        catch (WaitTaskTimeoutException)
        {
            return false;
        }
    }
}
