Feature: LogIntoPayPal
	Need to test logging in with invalid credentials

Background: Navigate to webpage
	Given user is on paypal log in page

@web @all
Scenario: Sign in with invalid email and password
	When invalid email and password are entered
		| email  | password    |
		| random | iLikeToTest |
	Then user gets error message
		| errorMessage                                              |
		| Some of your information isn't correct. Please try again. |

@web @all
Scenario: Sign in with unconfirmed phone number
	When unconfirmed phone number is entered
	Then user gets error message
		| errorMessage                                                                         |
		| You haven’t confirmed your mobile yet. Use your email for now. |

@web @all
Scenario: Try to log in with blank email
	When user clicks on next button
	Then user is told that email is required


#some more valid scenarios - use email address more than 3 times user gets locked out
#different error message when email used more than once