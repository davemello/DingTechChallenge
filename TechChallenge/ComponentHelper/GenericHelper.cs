using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using SeleniumProject.Settings;

namespace SeleniumProject.ComponentHelper
{
    public class GenericHelper : BaseComponentHelper
    {
        public static void TakeScreenShotAsJpeg(string filename = "Screen")
        {
            if (ObjectRepository.Driver != null)
            {
                Screenshot screen = ObjectRepository.Driver.TakeScreenshot();
                if (filename.Equals("Screen"))
                {
                    string name = filename + DateTime.UtcNow.ToString("yyyy-MM-dd-mm-ss") + ".jpeg";
                    screen.SaveAsFile(name, ScreenshotImageFormat.Jpeg);
                    return;
                }
                screen.SaveAsFile(filename, ScreenshotImageFormat.Jpeg);
            }
            
        }
    
    }
}
