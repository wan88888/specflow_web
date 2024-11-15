using TechTalk.SpecFlow;
using WebDemo.Utils;

namespace WebDemo
{
    [Binding]
    public sealed class Hooks : BrowserHelper
    {
        [BeforeScenario]
        public async Task FirstBeforeScenario()
        {
            await Init();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _browser.CloseAsync();
        }
    }
}