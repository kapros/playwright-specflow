using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BoDi;
using Microsoft.Playwright;
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
        private static IPlaywright _pw;

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
            _pw =  global::Microsoft.Playwright.Playwright.CreateAsync().Result;
        }

        [AfterTestRun]
        public static void GlobalTeardown()
        {
            _report.Flush();
        }

        [BeforeScenario]
        public async Task NavigateTo()
        {
            var browser = await _pw.Chromium.LaunchAsync(new global::Microsoft.Playwright.BrowserTypeLaunchOptions { Headless = false, SlowMo = 2000 });
            var browserContext = await browser.NewContextAsync(new BrowserNewContextOptions { BypassCSP = true });
            var page = await browserContext.NewPageAsync();
            await page.GotoAsync("http://todomvc.com/examples/mithril/#/");
            _objectContainer.RegisterInstanceAs(browser);
            _objectContainer.RegisterInstanceAs(page);
            var currentTest = _scenarioContext.ScenarioInfo.Title;
            _test = _report.CreateTest(currentTest);
            _currentTest = _test.CreateNode(currentTest);
        }

        [AfterScenario]
        public async Task SaveScreenshot()
        {
            if (_scenarioContext.TestError != null)
            {
                var dir = _resultsRoot.CreateSubdirectory("Screenshots");
                var screenshot = await _objectContainer.Resolve<IPage>().ScreenshotAsync(new PageScreenshotOptions { Path = System.IO.Path.Join(dir.FullName, $"{_scenarioContext.ScenarioInfo.Title}.jpg") });
                ReportTest(Convert.ToBase64String(screenshot));
            }
            await _objectContainer.Resolve<IBrowser>().CloseAsync();
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