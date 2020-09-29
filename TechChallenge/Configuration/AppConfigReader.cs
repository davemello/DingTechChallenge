using SeleniumProject.Interfaces;
using SeleniumProject.Settings;
using System;
using System.Configuration;
using NUnit.Framework;

namespace SeleniumProject.Configuration
{
    /// <summary>
    /// Retrieves specified values from app.config file using configuration manager
    /// </summary>
    public class AppConfigReader : IConfig
    {
        public BrowserType GetBrowser()
        {
            //will look at passed in nunit parameters to get browser, if none then will use default browser from app.config
            var browser = TestContext.Parameters.Get("BROWSER", ConfigurationManager.AppSettings.Get(AppConfigKeys.Browser));
            return (BrowserType)Enum.Parse(typeof(BrowserType), browser);
        }

        public int GetDefaultWebDriveWaitTimeout()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings.Get(AppConfigKeys.DefaultWebDriverWait));
        }

        public int GetElementLoadTimeout()
        {
            var timeout = ConfigurationManager.AppSettings.Get(AppConfigKeys.ElementLoadTimeout);
            if (timeout == null)
            {
                return 30;
            }
            return Convert.ToInt32(timeout);
        }

        public int GetPageLoadTimeOut()
        {
            var timeout = ConfigurationManager.AppSettings.Get(AppConfigKeys.PageLoadTimeout);
            if (timeout == null)
            {
                return 30;
            }
            return Convert.ToInt32(timeout);

        }

        public string GetWebsite()
        {
            return ConfigurationManager.AppSettings.Get(AppConfigKeys.Website);
        }

        public string GetRestEndPoint()
        {
            return ConfigurationManager.AppSettings.Get(AppConfigKeys.RestEndPoint);
        }

    }
}
