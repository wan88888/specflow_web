using Reqnroll;
using SpecFlowWeb.Tests.Drivers;

namespace SpecFlowWeb.Tests.Hooks;

[Binding]
public sealed class Hooks
{
    private static BrowserDriver? _browserDriver;
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static async Task BeforeTestRunAsync()
    {
        var headless = !string.Equals(
            Environment.GetEnvironmentVariable("HEADED"),
            "true",
            StringComparison.OrdinalIgnoreCase);

        _browserDriver = new BrowserDriver();
        await _browserDriver.StartAsync(headless);

        if (!headless)
        {
            Console.WriteLine("Running in headed mode with Google Chrome.");
        }
    }

    [AfterTestRun]
    public static async Task AfterTestRunAsync()
    {
        if (_browserDriver is not null)
        {
            await _browserDriver.DisposeAsync();
            _browserDriver = null;
        }
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        if (_browserDriver is null)
        {
            throw new InvalidOperationException("Browser was not started. Check BeforeTestRun hook.");
        }

        _scenarioContext.Set(_browserDriver);
    }
}
