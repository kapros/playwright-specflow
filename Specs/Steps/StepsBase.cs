using Playwright.PageObjects;
using Microsoft.Playwright;

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