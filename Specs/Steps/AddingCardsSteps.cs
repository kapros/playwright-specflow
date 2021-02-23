using NUnit.Framework;
using Playwright.PageObjects;
using PlaywrightSharp;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Playwright.Specs.Steps
{
    [Binding]
    public class AddingCardsSteps : StepsBase
    {
        public AddingCardsSteps(IPage page, ToDoMvcPage todoPage) : base(page, todoPage)
        { }

        [Given(@"Andrew has navigated to the app")]
        public void VisitPageAsync()
        {
            // step is empty as syntactic sugar
            // essentially, all tests require visiting the page
            // therefore that is done in a common setup binding
        }

        [When(@"Andrew adds a new card")]
        public async Task AddNewCardAsync()
        {
            await _todoPage.AddNewCard("card");
        }

        [Then(@"the card should appear as active")]
        public async Task AssertCardIsActiveAsync()
        {
            var allCards = await _todoPage.AllToDos;

            Assert.AreEqual(1, allCards.Count());
        }

        [Given(@"Andrew has navigated to the app for the first time")]
        public void VisitAppFirstTimeAsync()
        {
            // this is syntactic sugar as well
            // theoretically, this step could imitate first time navigation by making sure
            // local storage is cleaned
        }

        [Then(@"there should be no cards")]
        public async Task AssertNoCardsArePresentAsync()
        {
            var allCards = await _todoPage.AllToDos;

            Assert.AreEqual(0, allCards.Count());
        }

        [When(@"Andrew adds a new card with the task ""([^""]*)""")]
        public async Task AddNewCardNamedAsync(string todo)
        {
            await _todoPage.AddNewCard(todo);
        }

        [Then(@"the new card is present")]
        public async Task AssertCardIsPresent()
        {
            var allCards = await _todoPage.AllToDos;

            Assert.AreEqual(1, allCards.Count());
        }

        [Then(@"the card appears as active")]
        public async Task SwitchActiveAndAssertCardIsPresentAsync()
        {
            await _todoPage.SwitchToActive();
            var allCards = await _todoPage.AllToDos;

            Assert.AreEqual(1, allCards.Count());
        }
    }
}