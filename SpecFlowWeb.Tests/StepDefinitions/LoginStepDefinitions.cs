using Reqnroll;
using SpecFlowWeb.Tests.Drivers;
using SpecFlowWeb.Tests.Pages;

namespace SpecFlowWeb.Tests.StepDefinitions;

[Binding]
public sealed class LoginStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private LoginPage? _loginPage;
    private InventoryPage? _inventoryPage;

    public LoginStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private BrowserDriver BrowserDriver => _scenarioContext.Get<BrowserDriver>();

    [Given(@"I am on the SauceDemo login page")]
    public async Task GivenIAmOnTheSauceDemoLoginPage()
    {
        _loginPage = new LoginPage(BrowserDriver.Page);
        await _loginPage.NavigateAsync();
    }

    [When(@"I log in with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenILogInWithUsernameAndPassword(string username, string password)
    {
        Assert.That(_loginPage, Is.Not.Null, "login page must be opened first");
        await _loginPage!.LoginAsync(username, password);
        _inventoryPage = new InventoryPage(BrowserDriver.Page);
    }

    [Then(@"I should see the products inventory page")]
    public async Task ThenIShouldSeeTheProductsInventoryPage()
    {
        Assert.That(_inventoryPage, Is.Not.Null, "login must have been attempted");
        var isDisplayed = await _inventoryPage!.IsDisplayedAsync();
        Assert.That(isDisplayed, Is.True, "user should land on the inventory page after a successful login");
    }

    [Then(@"the page title should be ""(.*)""")]
    public async Task ThenThePageTitleShouldBe(string expectedTitle)
    {
        Assert.That(_inventoryPage, Is.Not.Null, "inventory page must be available");
        var actualTitle = await _inventoryPage!.GetTitleAsync();
        Assert.That(actualTitle, Is.EqualTo(expectedTitle));
    }
}
