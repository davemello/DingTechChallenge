using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumProject.ComponentHelper;
using System;
using System.Linq;
using System.Threading;
using TechChallenge.ComponentHelper;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SeleniumProject.PageObject
{
    public class LogInPage : BasePage
    {

        private readonly IWebDriver _driver;

        [FindsBy(How = How.Id, Using = "email")]
        private readonly IWebElement EmailTextBox;

        [FindsBy(How = How.Id, Using = "btnNext")]
        private readonly IWebElement NextButton;

        [FindsBy(How = How.Id, Using = "acceptAllButton")]
        private readonly IWebElement AcceptCookiesButton;

        [FindsBy(How = How.Id, Using = "password")]
        private readonly IWebElement PasswordTextBox;

        [FindsBy(How = How.Id, Using = "btnLogin")]
        private readonly IWebElement LogInButton;

        [FindsBy(How = How.XPath, Using = "(//p[@class='notification notification-critical'])[1]")]
        private readonly IWebElement IncorrectTextErrorNotification;

        [FindsBy(How = How.XPath, Using = "((//p[@class='notification notification-warning'])[1]")]
        private readonly IWebElement NonConfirmedMobileNotification;

        readonly WebDriverWait wait;
        string NotificationText;

        public LogInPage(IWebDriver driver) : base(driver)
        {
            if (driver != null)
            {
                _driver = driver;
                Logger.Info("Set implicit and explicit wait times");
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            }
            
        }

        internal void EnterPhoneNumber()
        {
            var phoneNo = CreateRandomNumberUsingUnixTimeStamp();
            bool IsNextButtonVisible = true;
            do
            {
                try
                {
                    wait.Until(ElementDisplayed(EmailTextBox));
                }
                catch (WebDriverTimeoutException)
                {
                    Logger.Warn("Text box not found");
                }
                TextBoxHelper.ClickAndTypeInTextBox(EmailTextBox, phoneNo.ToString());
                ButtonHelper.ClickButton(NextButton);
                Thread.Sleep(1000);

                //check if warning has appeared
                try
                {
                    if (_driver.FindElements(By.XPath("(//p[@class='notification notification-warning'])[1]")).Count == 1)
                    {
                        NotificationText = _driver.FindElement(By.XPath("(//p[@class='notification notification-warning'])[1]")).Text;
                        Logger.Info($"Notification text set to: {NotificationText}");
                        return;
                    }
                }
                catch (Exception)
                {
                    Logger.Info("Warning has not appeared");
                }

                try
                {
                    wait.Until(ElementDisplayed(NextButton));
                    if (!NextButton.Displayed)
                    {
                        IsNextButtonVisible = false;
                    }
                }
                catch (Exception)
                {
                    IsNextButtonVisible = false;
                }

            } while (IsNextButtonVisible);
        }

        internal bool IsWarningTextDisplayedInRedIfTextFieldBlank()
        {

            if (NextButton.Displayed)
            {
                ButtonHelper.ClickButton(NextButton);
            }
            Logger.Info("Email required error should be shown if field is blank");
            var color = _driver.FindElement(By.XPath("(//*[@class='errorMessage'])[1]")).GetCssValue("color");
            if(color.Equals("rgba(255, 255, 255, 1)"))
            {
                return true;
            }
            return false;
            
        }

        internal void EnterEmailAndPassword(Table table)
        {
            var valuesFromTable = table.CreateDynamicSet();
            string password = valuesFromTable.First().password;
            string email = valuesFromTable.First().email;
            if (email.Equals("random"))
            {
                email = CreateRandomEmail();
            }

            Logger.Info($"Email is: {email}; Password: {password}");
            bool IsNextButtonVisible = true;
            do
            {
                try
                {
                    wait.Until(ElementDisplayed(EmailTextBox));
                }
                catch (WebDriverTimeoutException)
                {
                    Logger.Warn("Text box not found");
                }
                TextBoxHelper.ClickAndTypeInTextBox(EmailTextBox, email);
                ButtonHelper.ClickButton(NextButton);
                Thread.Sleep(1000);
                try
                {
                    wait.Until(ElementDisplayed(NextButton));
                    if (!NextButton.Displayed)
                    {
                        IsNextButtonVisible = false;
                    }
                }
                catch (Exception)
                {
                    IsNextButtonVisible = false;
                }

            } while (IsNextButtonVisible);

            try
            {
                wait.Until(ElementDisplayed(PasswordTextBox));
                TextBoxHelper.ClickAndTypeInTextBox(PasswordTextBox, password);
            }
            catch (Exception)
            {
                Logger.Warn("Password box not found");
            }
            LogInButton.Click();
            NotificationText = IncorrectTextErrorNotification.Text;

        }

        internal string GetNotificationText()
        {
            return NotificationText;
        }

        internal void ClickLogInButton()
        {
            ButtonHelper.ClickButton(LogInButton);

        }

        internal void ClickNextButton()
        {
            ButtonHelper.ClickButton(NextButton);
        }

        #region private methods
        private string CreateRandomEmail()
        {
            return $"{CreateRandomNumberUsingUnixTimeStamp()}@tryagain.com";
        }

        private long CreateRandomNumberUsingUnixTimeStamp()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            return now.ToUnixTimeMilliseconds();
        }
        private Func<IWebDriver, bool> TextBoxIsEnabled(IWebElement emailTextBox)
        {
            return x => emailTextBox.Enabled;
        }

        private Func<IWebDriver, bool> ElementDisplayed(By locator)
        {
            return x => x.FindElement(locator).Displayed;
        }

        private Func<IWebDriver, bool> ElementDisplayed(IWebElement element)
        {
            return x => element.Displayed;
        }

        #endregion
    }
}
