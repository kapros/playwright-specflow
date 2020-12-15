Feature: CompletingToDos

As a user
I want to be able to complete my todos
In order to see what activities are left to do

@CompletingTodos
Scenario: Marking cards as completed
	Given Andrew has finished a task
	When Andrew marks a card as completed
	Then the card should appear as completed

@CompletingTodos
Scenario: Completed cards persist
	Given Andrew has marked a card as completed
	When Andrew reloads the page
	Then the card should appear as completed