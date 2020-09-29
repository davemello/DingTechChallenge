using FluentAssertions;
using SeleniumProject.ComponentHelper;
using SeleniumProject.Settings;
using SeleniumProject.StepDefinition;
using SeleniumProject.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TechChallenge.StepDefinition
{
    [Binding]
    public sealed class LogInPageDefinition : BaseDefinition
    {
        [Given(@"user is on paypal log in page")]
        public void GivenUserIsOnPaypalLogInPage()
        {
            NavigationHelper.NavigateToUrl(ObjectRepository.Config.GetWebsite());
            JavaScriptExecutor.WaitForPageLoad(ObjectRepository.Driver);
        }

        [When(@"invalid email and password are entered")]
        public void WhenInvalidEmailAndPasswordAreEntered(Table table)
        {
            LogInPage.EnterEmailAndPassword(table);
        }

        [When(@"user clicks on log in button")]
        public void WhenUserClicksOnLogInButton()
        {
            LogInPage.ClickLogInButton();
        }

        [When(@"user clicks on next button")]
        public void WhenUserClicksOnNextButton()
        {
            LogInPage.ClickNextButton();
        }


        [When(@"unconfirmed phone number is entered")]
        public void WhenUnconfirmedPhoneNumberIsEntered()
        {
            LogInPage.EnterPhoneNumber();
        }

        [Then(@"user gets error message")]
        public void ThenUserGetsErrorMessage(Table table)
        {
            var set = table.CreateDynamicSet();
            var expectedMsg = set.First().errorMessage;
            var actualText = LogInPage.GetNotificationText();
            actualText.Should().Be(expectedMsg);
        }

        [Then(@"textbox display text turns red as a warning")]
        public void ThenTextboxDisplayTextTurnsRedAsAWarning()
        {
            LogInPage.IsWarningTextDisplayedInRedIfTextFieldBlank().Should().BeTrue();
        }






    }
}
