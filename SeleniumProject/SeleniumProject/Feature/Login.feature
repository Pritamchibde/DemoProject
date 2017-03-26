Feature: Login 
	In order to login to the system
	As a demo user
	I want to be on the demo page

@Selenium
Scenario: Login as Demo user
	Given I am on the login page
	When I enter username and password 
	And Click on Login button
	Then User should be logged in into the system
