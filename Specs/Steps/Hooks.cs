using BoDi;
using PlaywrightSharp;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Playwright.Specs.Steps
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task NavigateTo()
        {
            var pwright = await global::PlaywrightSharp.Playwright.CreateAsync();
            var browser = await pwright.Chromium.LaunchAsync(new LaunchOptions { Headless = false });
            var browserContext = await browser.NewContextAsync(new BrowserContextOptions { BypassCSP = true });
            var page = await browserContext.NewPageAsync();
            await page.GoToAsync("http://todomvc.com/examples/mithril/#/");
            _objectContainer.RegisterInstanceAs(browser);
            _objectContainer.RegisterInstanceAs(page);
        }

        [AfterScenario]
        public async Task SaveScreenshot()
        {
            if (_scenarioContext.TestError == null)
                return;
            var dir = System.IO.Directory.CreateDirectory("Screenshots");
            await _objectContainer.Resolve<IPage>().ScreenshotAsync(System.IO.Path.Join(dir.FullName, _scenarioContext.ScenarioInfo.Title));
        }
    }
}