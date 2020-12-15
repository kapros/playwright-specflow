using Playwright.PageObjects;
using PlaywrightSharp;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Playwright
{
    public class StepsBase
    {
        protected readonly IPage _page;
        protected readonly ToDoMvcPage _todoPage;

        public StepsBase(IPage page, ToDoMvcPage todoPage)
        {
            _page = page;
            _todoPage = todoPage;
        }
    }
}