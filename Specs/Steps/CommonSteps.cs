﻿using Playwright.PageObjects;
using Microsoft.Playwright;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Playwright.Specs.Steps
{
    [Binding]
    public class CommonSteps : StepsBase
    {
        public CommonSteps(IPage page, ToDoMvcPage todoPage) : base(page, todoPage)
        {
        }

        [When(@"Andrew reloads the page")]
        public async Task ReloadPageAsync()
        {
            await _page.ReloadAsync();
        }
    }
}