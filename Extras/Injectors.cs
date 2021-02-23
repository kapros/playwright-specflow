using PlaywrightSharp;
using System.Dynamic;
using System.Threading.Tasks;

namespace Playwright.Extras
{
    public static class Injectors
    {
        public const string CardTitle = "todo1";
        public const bool CardCompletedStatus = true;
        public const bool CardEditingStatus = false;
        public const int CardKey = 1;

        public static async Task InjectExisintgToDoCard(IPage page)
        {
            var key = "todos-mithril";
            dynamic value = new ExpandoObject();
            value.title = CardTitle;
            value.completed = CardCompletedStatus;
            value.editing = CardEditingStatus;
            value.key = CardKey;
            var stringified = Newtonsoft.Json.JsonConvert.SerializeObject(new[] { value });
            await page.EvaluateAsync($"window.localStorage.setItem('{key}','{stringified}')");
            await page.ReloadAsync();
            await page.ClickAsync("a[href*='completed']");
        }
    }
}