using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using OpenQA.Selenium.Interactions;
using System.Xml;
using System.Text.RegularExpressions;
using ActiveUp.Net.Mail;
using ActiveUp.Net.Imap4;

namespace SeleniumProject.Pages
{
    class Extension : Global
    {
        public string basewindowhandle = WebBrowser.driver.CurrentWindowHandle;
        public Imap4Client _client = null;
        public Mailbox mails = null;

        #region CoreOverriddenMethods
        public void Click(By Locator)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            WebBrowser.driver.FindElement(Locator).Click();
        }

        public void ClickByReference(By ReferenceLocator, By ActionLocator)
        {
            WaitUntilIsElementExistsAndDisplayed(ReferenceLocator);
            IWebElement element1 = WebBrowser.driver.FindElement(ReferenceLocator);
            element1.FindElement(ActionLocator).Click();
        }

        public void HoverAndClick(By Locator)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            Actions actions = new Actions(WebBrowser.driver);
            IWebElement element = WebBrowser.driver.FindElement(Locator);
            actions.MoveToElement(element);
            actions.Click().Perform();
            actions.Release().Perform();
        }

        public void Hover(By Locator)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            Actions actions = new Actions(WebBrowser.driver);
            IWebElement element = WebBrowser.driver.FindElement(Locator);
            actions.MoveToElement(element);
            actions.Build().Perform();
        }

        public void JsExecutorClick(By Locator, string javascript)
        {
            IWebElement element = WebBrowser.driver.FindElement(Locator);
            ((IJavaScriptExecutor)WebBrowser.driver).ExecuteScript(javascript, element);
            element.Click();
        }

        public void DismissAlert()
        {
            IAlert alert = WebBrowser.driver.SwitchTo().Alert();
            alert.Dismiss();
        }

        public void AcceptAlert()
        {
            IAlert alert = WebBrowser.driver.SwitchTo().Alert();
            alert.Accept();
        }

        public Boolean isAlertPresent()
        {
            try
            {
                WebBrowser.driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException ex)
            {
                return false;
            }
        }

        public void SelectElementByText(By Locator, string text)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            new SelectElement(WebBrowser.driver.FindElement(Locator)).SelectByText(text);
        }

        public void SelectElementByValue(By Locator, string value)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            new SelectElement(WebBrowser.driver.FindElement(Locator)).SelectByValue(value);
        }

        public int SelectElementOptionCount(By Locator)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            IWebElement Group = WebBrowser.driver.FindElement(Locator);
            SelectElement GroupSelect = new SelectElement(Group);
            var OptionCount = GroupSelect.Options.Count;
            return OptionCount;
        }

        public void SendKeys(By Locator, string text)
        {
            Clear(Locator);
            WebBrowser.driver.FindElement(Locator).SendKeys(text);
            waitForLoad();
        }

        public void Clear(By Locator)
        {
            WaitUntilIsElementExistsAndDisplayed(Locator);
            WebBrowser.driver.FindElement(Locator).Clear();
        }

        public void NavigateToUrl(string url)
        {
            WebBrowser.driver.Navigate().GoToUrl(BaseUrl + url);
            waitForLoad();
        }

        #endregion

        #region Assertions
        public void WaitUntilIsElementExistsAndDisplayed(By Locator)
        {
            waitForLoad();
            try
            {
                WebDriverWait wait = new WebDriverWait(WebBrowser.driver, new TimeSpan(0, 0, WebDriverExplictTimeoutSeconds));
                wait.Until(ExpectedConditions.ElementExists(Locator));
                wait.Until(ExpectedConditions.ElementIsVisible(Locator));
            }
            catch (Exception ex)
            {
                Assert.Fail("Following element is not displayed : " + Locator);
            }
        }

        public void WaitUntilIsElementExists(By Locator)
        {
            waitForLoad();
            try
            {
                WebDriverWait wait = new WebDriverWait(WebBrowser.driver, new TimeSpan(0, 0, WebDriverExplictTimeoutSeconds));
                wait.Until(ExpectedConditions.ElementExists(Locator));
            }
            catch (Exception ex)
            {
                Assert.Fail("Following element is not displayed : " + Locator);
            }
        }

        public bool IsElementPresent(IWebDriver driver, By locator)
        {
            waitForLoad();
            //Set the timeout to something low
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(PauseTime * 10));
            try
            {
                Thread.Sleep(PauseTime * 3);
                driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public void waitForLoad()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(WebBrowser.driver, new TimeSpan(0, 0, ReadyStateTimeOutSeconds));
                wait.Until(driver1 => ((IJavaScriptExecutor)WebBrowser.driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex)
            {
                //  StopPageLoad();
            }
        }
        public void StopPageLoad()
        {
            ((IJavaScriptExecutor)WebBrowser.driver).ExecuteScript("return document.execCommand('Stop');");
        }

        #endregion

        #region HandlingComplexEvents
        public void SwitchToPopup(string WindowTitle, IWebDriver driver)
        {
            string BaseWindow = driver.CurrentWindowHandle;
            ReadOnlyCollection<string> handles = driver.WindowHandles;
            int count = handles.Count;
            foreach (string handle in handles)
            {
                if (handle != BaseWindow)
                {
                    //Switching to the PDF window that contain title present in WindowTitle variable
                    driver.SwitchTo().Window(handle);
                    driver.Manage().Window.Maximize();
                    if (driver.Url.Contains(WindowTitle) && count == 2)
                        break;
                    else if (driver.Url.Contains(WindowTitle) && count == 3)
                    {
                        driver.Close();
                        count--;
                    }
                }
            }
        }

        public void SwitchToBaseWindow(IWebDriver driver)
        {
            driver.SwitchTo().Window(basewindowhandle);
            driver.Manage().Window.Maximize();
            Thread.Sleep(PauseTimeLonger);
        }

        public void SwitchToLastPopUp(IWebDriver driver)
        {
            int count = driver.WindowHandles.ToList().Count;
            while (--count >= 0)
            {
                if (basewindowhandle != driver.WindowHandles[count])
                {
                    waitForLoad();
                    driver.SwitchTo().Window(driver.WindowHandles[count]);
                    driver.Manage().Window.Maximize();
                    break;
                }
            }
            Thread.Sleep(PauseTimeLonger * 5);
        }
        public string ExtractTextFromPdf(IWebDriver driver)
        {
            Thread.Sleep(PauseTimeLonger * 5);
            waitForLoad();
            using (PdfReader reader = new PdfReader(driver.Url))
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }
        }
        #endregion

    }
}

