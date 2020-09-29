using OpenQA.Selenium;
using SeleniumProject.ComponentHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge.ComponentHelper
{
   public class TextBoxHelper: BaseComponentHelper
    {
        public static void TypeInTextBox(IWebElement element, string text)
        {
            Logger.Info($"Sending text: {text}");
            element.SendKeys(text);

        }

        public static void ClickAndTypeInTextBox(IWebElement element, string text)
        {
            Logger.Info($"Sending text: {text}");
            element.Click();
            element.SendKeys(text);

        }
    }
}
