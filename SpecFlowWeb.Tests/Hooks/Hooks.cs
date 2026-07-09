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
        _browserDriver = new BrowserDriver();
        await _browserDriver.StartAsync(headless: !BrowserDriver.IsHeadedMode);
    }

    [AfterTestRun]
    public static async Task AfterTestRunAsync()
    {
        if (_browserDriver is null)
        {
            return;
        }

        if (BrowserDriver.IsHeadedMode
            && BrowserDriver.TryGetHeadedPauseMilliseconds(out var pauseMs)
            && pauseMs > 0)
        {
            Console.WriteLine($"Headed mode: keeping browser open for {pauseMs}ms...");
            await Task.Delay(pauseMs);
        }

        await _browserDriver.DisposeAsync();
        _browserDriver = null;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _scenarioContext.Set(_browserDriver!);
    }
}
