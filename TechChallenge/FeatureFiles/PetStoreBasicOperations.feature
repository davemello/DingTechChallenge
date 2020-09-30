Feature: PetStoreBasicOperations
	New API created which allows user to add, read, update and delete pet details.
	Tests should check basic functionality

@api @all
Scenario: Test basic CRUD operations

	Given user creates a pet
	When user calls GET method
	Then user can read the pet details which match the details that were used to create pet
	And user updates pet details
	When user calls GET method
	Then user can read the pet details which match the details that were used to update pet
	And delete pet details when complete

