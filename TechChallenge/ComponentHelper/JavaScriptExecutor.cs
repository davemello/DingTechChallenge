using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumProject.Settings;
using System;

namespace SeleniumProject.ComponentHelper
{
    public class JavaScriptExecutor : BaseComponentHelper
    {
      
        public static object ExecuteScript(string script)
        {
            var executor = (IJavaScriptExecutor)ObjectRepository.Driver;
            Logger.Info($" Execute Script @ {script}");
            return executor.ExecuteScript(script);
        }

        public static void WaitForPageLoad(IWebDriver webDriver)
        {
            var status = (string)((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState");
            Logger.Info($"Page load status: {status}");
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            wait.Until(drv => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}


