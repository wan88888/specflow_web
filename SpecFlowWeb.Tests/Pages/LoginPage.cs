using PuppeteerSharp;

namespace SpecFlowWeb.Tests.Pages;

public sealed class LoginPage
{
    private const string Url = "https://www.saucedemo.com/";
    private const string UsernameSelector = "#user-name";
    private const string PasswordSelector = "#password";
    private const string LoginButtonSelector = "#login-button";

    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync()
    {
        await _page.GoToAsync(Url, new NavigationOptions
        {
            WaitUntil = [WaitUntilNavigation.DOMContentLoaded]
        });
        await _page.WaitForSelectorAsync(UsernameSelector);
    }

    public async Task LoginAsync(string username, string password)
    {
        await FillInputAsync(UsernameSelector, username);
        await FillInputAsync(PasswordSelector, password);
        await _page.ClickAsync(LoginButtonSelector);
    }

    private async Task FillInputAsync(string selector, string value)
    {
        await _page.WaitForSelectorAsync(selector);
        await _page.EvaluateFunctionAsync(
            @"(sel) => { const el = document.querySelector(sel); if (el) el.value = ''; }",
            selector);
        await _page.TypeAsync(selector, value);
    }
}
