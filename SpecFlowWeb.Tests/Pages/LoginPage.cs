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
        await _page.GoToAsync(Url, WaitUntilNavigation.Networkidle2);
        await _page.WaitForSelectorAsync(UsernameSelector);
    }

    public async Task EnterUsernameAsync(string username)
    {
        await FillInputAsync(UsernameSelector, username);
    }

    public async Task EnterPasswordAsync(string password)
    {
        await FillInputAsync(PasswordSelector, password);
    }

    public async Task ClickLoginAsync()
    {
        await _page.ClickAsync(LoginButtonSelector);
    }

    public async Task LoginAsync(string username, string password)
    {
        await EnterUsernameAsync(username);
        await EnterPasswordAsync(password);
        await ClickLoginAsync();
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
