using NUnit.Framework;
using Playwright.Extras;
using Playwright.PageObjects;
using Microsoft.Playwright;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Playwright
{
    [Binding]
    public class CompletingCardsSteps : StepsBase
    {
        public const string CARD_NAME_COMPLETE = "complete";

        public CompletingCardsSteps(IPage page, ToDoMvcPage todoPage) : base(page, todoPage)
        { }

        [Given(@"Andrew has finished a task")]
        public async Task AddCardToMarkAsCompleteAsync()
        {
            // potentially, this could be merged with its counterpart from the adding cards feature
            // depending on the robustness of the specifications we arrive at
            // at this time, this is a consiouc decision to keep it as is mostly due to the name of the task
            await _todoPage.AddNewCard(CARD_NAME_COMPLETE);
        }

        [When(@"Andrew marks a card as completed")]
        public async Task MarkCardAsDoneAsync()
        {
            await _todoPage.MarkToDoAsDone(CARD_NAME_COMPLETE);
        }

        [Then(@"the card should appear as completed")]
        public async Task SwitchCompletedAndAssertCardIsPresentAsync()
        {
            await _todoPage.SwitchToCompleted();

            var allCards = await _todoPage.AllToDos;

            Assert.AreEqual(1, allCards.Count());
        }

        [Given(@"Andrew has marked a card as completed")]
        public async Task InjectExistingCompletedCardAsync()
        {
            await Injectors.InjectExisintgToDoCard(_page);
        }
    }
}