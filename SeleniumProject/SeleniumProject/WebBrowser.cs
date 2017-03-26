using System;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Interactions;
using System.Collections;
using SeleniumProject.Pages;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using System.Diagnostics;

namespace SeleniumProject
{
    [Binding]
    public static class WebBrowser
    {
        public static IWebDriver driver;
        public static string TestUrl = ConfigurationManager.AppSettings["BASEURL"] + "login";
        public static string IEDriverServer = ConfigurationManager.AppSettings["IE_DRIVER_PATH"];
        public static string EmailReceipints = ConfigurationManager.AppSettings["EMAIL_RECEIPENTS"];
        public static string EmailReceipintsFailure = ConfigurationManager.AppSettings["EMAIL_RECEIPENTS_FAILURE"];
        public static string emailfrom = ConfigurationManager.AppSettings["EMAIL_FROM"];
        public static string emailhostusername = ConfigurationManager.AppSettings["EMAILHOST_USERNAME"];
        public static string emailhostpassword = ConfigurationManager.AppSettings["EMAILHOST_PASSWORD"];
        public static string exchangeserver = ConfigurationManager.AppSettings["EXCHANGE_SERVER"];
        public static Int32 ImplicitlyWaitTimeoutSeconds = Int32.Parse(ConfigurationManager.AppSettings["ImplicitlyWaitTimeoutSeconds"]);
        public static Int32 SetScriptTimeoutSeconds = Int32.Parse(ConfigurationManager.AppSettings["SetScriptTimeoutSeconds"]);
        public static Int32 WebDriverExplictTimeoutSeconds = Int32.Parse(ConfigurationManager.AppSettings["SetPageLoadTimeoutSeconds"]);    
     
      
        [BeforeScenario("Selenium")]
        public static void setupselenium()
        {          
            // Delete any cached task list that persists even after a regression database refresh. 
            System.Web.HttpRuntime.UnloadAppDomain();
            try
            {
              IEDriverSetup();
            }
            catch(Exception ex)
            {
                driver.Dispose();
                IEDriverSetup();
            }
        }     
        [BeforeScenario("Selenium_NativeElements")]
        public static void setupseleniumForNativeElements()
        {
            // Delete any cached task list that persists even after a regression database refresh. 
            System.Web.HttpRuntime.UnloadAppDomain();
            try
            {
            IEDriverSetupNativeElements();
            }
            catch (Exception ex)
            {
                driver.Dispose();
                IEDriverSetupNativeElements();
            }           
        }
        public static void IEDriverSetup()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.IgnoreZoomLevel = true;
            options.EnablePersistentHover = false;
            options.EnableNativeEvents = false;
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            options.PageLoadStrategy = InternetExplorerPageLoadStrategy.Normal;
            options.EnsureCleanSession = true;
            LaunchBrowserAndLoadApplication(IEDriverServer, options);
        }
        public static void IEDriverSetupNativeElements()
        {
            KillIE();
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.IgnoreZoomLevel = true;
            options.EnablePersistentHover = true;
            options.EnableNativeEvents = true;
            options.EnsureCleanSession = true;
            LaunchBrowserAndLoadApplication(IEDriverServer, options);
        }
        public static void LaunchBrowserAndLoadApplication(string IEDriverServer, InternetExplorerOptions options)
        {
            driver = new InternetExplorerDriver(IEDriverServer, options);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(ImplicitlyWaitTimeoutSeconds));
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(SetScriptTimeoutSeconds));
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(WebDriverExplictTimeoutSeconds));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(TestUrl);
        }

        [AfterScenario("Selenium_NativeElements")]
        [AfterScenario("Selenium")]
        public static void CloseSelenium()
        {        
            driver.Dispose();                       
        }
 
       

        #region SupportingMethods
        public static void KillIE()
        {
            try
            {
                Process[] ieProcesses = Process.GetProcessesByName("iexplore");
                foreach (Process ie in ieProcesses)
                {
                    ie.CloseMainWindow();
                    ie.Close();
                    ie.Dispose();
                }
            }
            catch (Exception ex) { }
        }  
        public static void TakeScreenshot(IWebDriver driver, String filename)
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filename, ImageFormat.Png);
            }
            catch (Exception ex){}
        }
        internal static void WaitforSaving()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(d => !driver.FindElement(By.XPath("//*[@id='nfDialogSimple_Box']")).Displayed);
        }
        #endregion
    }
}
