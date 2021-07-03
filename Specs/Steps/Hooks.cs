using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BoDi;
using PlaywrightSharp;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Playwright.Specs.Steps
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private static ExtentReports _report = new();
        private static ExtentTest _test;
        private ExtentTest _currentTest;
        private static DirectoryInfo _resultsRoot;

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void GlobalSetup()
        {
            _resultsRoot = Directory.CreateDirectory("Results");
            _report.AttachReporter(new ExtentHtmlReporter(_resultsRoot.FullName,
        AventStack.ExtentReports.Reporter.Configuration.ViewStyle.SPA));
        }

        [AfterTestRun]
        public static void GlobalTeardown()
        {
            _report.Flush();
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
            var currentTest = _scenarioContext.ScenarioInfo.Title;
            _test = _report.CreateTest(currentTest);
            _currentTest = _test.CreateNode(currentTest);
        }

        [AfterScenario]
        public async Task SaveScreenshot()
        {
            if (_scenarioContext.TestError == null)
                return;
            var dir = _resultsRoot.CreateSubdirectory("Screenshots");
            var screenshot = await _objectContainer.Resolve<IPage>().ScreenshotAsync(System.IO.Path.Join(dir.FullName, $"{_scenarioContext.ScenarioInfo.Title}.jpg"));
            ReportTest(Convert.ToBase64String(screenshot));
        }

        private void ReportTest(string screenshot)
        {
            if (_scenarioContext.TestError is not null)
            {
                _currentTest.Fail($"{_scenarioContext.TestError.Message}. Stack trace:{Environment.NewLine}{_scenarioContext.TestError.StackTrace}")
                            .AddScreenCaptureFromBase64String(
                                screenshot);
                return;
            }
            _currentTest.Pass("");
        }
    }
}