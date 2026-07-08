Feature: SauceDemo Login
  As a SauceDemo customer
  I want to log in with valid credentials
  So that I can browse products

  Scenario: Successful login with standard user
    Given I am on the SauceDemo login page
    When I log in with username "standard_user" and password "secret_sauce"
    Then I should see the products inventory page
    And the page title should be "Products"
