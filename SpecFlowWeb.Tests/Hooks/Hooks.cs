using Reqnroll;
using SpecFlowWeb.Tests.Drivers;

namespace SpecFlowWeb.Tests.Hooks;

[Binding]
public sealed class Hooks
{
    private readonly ScenarioContext _scenarioContext;
    private BrowserDriver? _browserDriver;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task BeforeScenarioAsync()
    {
        var headless = !string.Equals(
            Environment.GetEnvironmentVariable("HEADED"),
            "true",
            StringComparison.OrdinalIgnoreCase);

        _browserDriver = new BrowserDriver();
        await _browserDriver.StartAsync(headless);
        _scenarioContext.Set(_browserDriver);
    }

    [AfterScenario]
    public async Task AfterScenarioAsync()
    {
        if (_browserDriver is not null)
        {
            await _browserDriver.DisposeAsync();
            _browserDriver = null;
        }
    }
}
