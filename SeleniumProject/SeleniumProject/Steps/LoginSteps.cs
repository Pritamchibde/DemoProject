using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SeleniumProject.Pages;
using TechTalk.SpecFlow;

namespace SeleniumProject.Steps
{
    [Binding]
    public class LoginSteps : Global
    {
        Extension extension = new Extension();
        IWebDriver driver = WebBrowser.driver;

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            extension.NavigateToUrl("Login");
        }

        [When(@"I enter username and password")]
        public void WhenIEnterUsernameAndPassword()
        {
            extension.SendKeys(By.Name("username"), DemoUserName);
            extension.SendKeys(By.Name("password"), DemoUserPassword);
        }

        [When(@"Click on Login button")]
        public void WhenClickOnLoginButton()
        {
            extension.Click(By.XPath("//button[text()='Login']"));
        }

        [Then(@"User should be logged in into the system")]
        public void ThenUserShouldBeLoggedInIntoTheSystem()
        {
            Assert.IsTrue(extension.IsElementPresent(driver, By.XPath("//h3[text()='Hi, John Smith']")));
        }
    }
}
