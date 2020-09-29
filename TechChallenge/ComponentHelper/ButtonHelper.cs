using OpenQA.Selenium;

namespace SeleniumProject.ComponentHelper
{
    public class ButtonHelper : BaseComponentHelper
    {
        public static void ClickButton(IWebElement element)
        {
            Logger.Info($"Clicking button: {element.Text}");
            element.Click();
        }
    }

}
