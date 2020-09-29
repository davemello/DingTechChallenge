using log4net;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using SeleniumProject.Logging;

namespace SeleniumProject.PageObject
{

    /// <summary>
    /// Holds an instance of logger and initialises elements using page factory
    /// </summary>
    public class BasePage
    {
        private readonly IWebDriver _driver;
        
        public string Title => _driver.Title;
        public static readonly ILog Logger = LogHelper.GetXmlLogger(typeof(BasePage));
        public BasePage(IWebDriver driver)
        {
            if (driver != null)
            {
                PageFactory.InitElements(driver, this);
                _driver = driver;
            }
            
        }

      }
}
