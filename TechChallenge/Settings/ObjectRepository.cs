using SeleniumProject.Interfaces;
using OpenQA.Selenium;
using RestSharp;

namespace SeleniumProject.Settings
{
    public static class ObjectRepository
    {
        public static IConfig Config { get; set; }

        public static IWebDriver Driver { get; set; }

        public static RestClient Client => new RestClient(Config.GetRestEndPoint());
    }
}
