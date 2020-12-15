using PlaywrightSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playwright.PageObjects
{
    public class ToDoMvcPage
    {
        private readonly IPage _page;

        public ToDoMvcPage(IPage page)
        {
            _page = page;
        }

        public Task<IEnumerable<IElementHandle>> AllToDos => _page.QuerySelectorAllAsync(".todo-list .view");

        public async Task MarkToDoAsDone(string todo)
        {
            var allCheckBoxes = await AllToDos;
            var todoCard = allCheckBoxes.FirstOrDefault(x => x.GetTextContentAsync().Result == todo);
            var checkbox = await todoCard.QuerySelectorAsync("input[type='checkbox']");
            await checkbox.CheckAsync();
        }

        public async Task AddNewCard(string todo)
        {
            var input = await _page.QuerySelectorAsync(".new-todo");
            await input.ClickAsync();
            await input.FillAsync(todo);
            await input.PressAsync("Enter");
        }

        public async Task SwitchToCompleted()
        {
            await _page.ClickAsync("a[href*='completed']");
        }

        public async Task SwitchToActive()
        {
            await _page.ClickAsync("a[href*='active']");
        }
    }
}
