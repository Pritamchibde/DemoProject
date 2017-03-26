using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;


namespace SeleniumProject
{
    public partial class Global
    {
        //URL
        public static string BaseUrl = ConfigurationManager.AppSettings["BASEURL"];
        public static string TestUrl = ConfigurationManager.AppSettings["BASEURL"] + "login";

        public static string DemoUserName = ConfigurationManager.AppSettings["Demo_USERNAME"];
        public static string DemoUserPassword = ConfigurationManager.AppSettings["Demo_PASSWORD"];

        public static int PauseTime = Int32.Parse(ConfigurationManager.AppSettings.Get("PAUSE_TIME"));
        public static int PauseTimeLonger = Int32.Parse(ConfigurationManager.AppSettings.Get("PAUSE_TIME_LONGER"));

        //Timeouts
       public static Int32 WebDriverExplictTimeoutSeconds = Int32.Parse(ConfigurationManager.AppSettings["WebDriverExplictTimeoutSeconds"]);
       public static Int32 ReadyStateTimeOutSeconds = Int32.Parse(ConfigurationManager.AppSettings["ReadyStateTimeOutSeconds"]); 
    }
}


