using TechTalk.SpecFlow;
using WebDemo.Utils;

namespace WebDemo.Step
{
    [Binding]
    public class BaiduSearchStep : BrowserHelper
    {
      
        [Given(@"I am on baidu.com")]
        public async Task GoWebsite()
        {
            await _page.GoToAsync("https://www.baidu.com");
            Thread.Sleep(2000);
        }

        [When(@"I enter ""(.*)"" in the search box")]
        public async Task EnterText(string searchText)
        {
            await _page.TypeAsync("#kw", searchText);
            Thread.Sleep(2000);
        }

        [When(@"I click search button")]
        public async Task ClickButton()
        {
            await _page.ClickAsync("#su");
            Thread.Sleep(2000);
        }
    }
}