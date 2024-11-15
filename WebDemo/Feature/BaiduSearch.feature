Feature: BaiduSearch

  Scenario: Search for Specflow on Baidu
    Given I am on baidu.com
    When I enter "Specflow" in the search box
    And I click search button