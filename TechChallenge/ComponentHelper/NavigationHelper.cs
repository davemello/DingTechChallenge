using SeleniumProject.Settings;


namespace SeleniumProject.ComponentHelper
{
    public class NavigationHelper : BaseComponentHelper
    {
        public static void NavigateToUrl(string Url)
        {
            Logger.Info($"Navigating to: {Url}");
            ObjectRepository.Driver.Navigate().GoToUrl(Url);
            ObjectRepository.Driver.Manage().Cookies.DeleteAllCookies();
            JavaScriptExecutor.WaitForPageLoad(ObjectRepository.Driver);
        }
    }
}
