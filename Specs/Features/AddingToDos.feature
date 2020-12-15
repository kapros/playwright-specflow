Feature: Adding cards feature

As the user
I would like to add cards
In order to keep track of their activities

@AddingCards
Scenario: Adding cards
	Given Andrew has navigated to the app
	When Andrew adds a new card
	Then the card should appear as active

@AddingCards
Scenario: New user
	Given Andrew has navigated to the app for the first time
	Then there should be no cards

@AddingCards
Scenario: New cards are persistent
	Given Andrew has navigated to the app
	When Andrew adds a new card with the task "todo"
	And Andrew reloads the page
	Then the new card is present
	And the card appears as active