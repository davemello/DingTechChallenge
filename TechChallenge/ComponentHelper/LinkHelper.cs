using OpenQA.Selenium;

namespace SeleniumProject.ComponentHelper
{
    public class LinkHelper : BaseComponentHelper
    {
        public static void ClickLink(IWebElement element)
        {
            Logger.Info($"Clicking on link with text: {element.Text}");
            if (element.Enabled)
            {
                element.Click();
            }
            else
            {
                Logger.Warn("Link is not enabled to click");
            }
        }
    }
}
