﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SeleniumProject.ComponentHelper;
using SeleniumProject.Configuration;
using SeleniumProject.CustomException;
using SeleniumProject.Settings;
using System;
using log4net;
using SeleniumProject.Logging;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;
using SeleniumProject.PageObject;
using System.IO;
using OpenQA.Selenium.Support.UI;
using AngleSharp.Text;

namespace SeleniumProject.StepDefinition
{
    [Binding]
    public class BaseDefinition
    {

        private static ScenarioContext _scenarioContext;
        private static FeatureContext _featureContext;
        private static ExtentReports _extentReports;
        private static ExtentHtmlReporter _extentHtmlReporter;
        private static ExtentTest _feature;
        private static ExtentTest _scenario;
        public static readonly ILog Logger = LogHelper.GetXmlLogger(typeof(BaseDefinition));

        public static LogInPage LogInPage;


        protected BaseDefinition()
        {
            LogInPage = new LogInPage(ObjectRepository.Driver);

        }

        [BeforeTestRun]
        public static void SetUpTest()
        {
            //set working dir for reports etc to bin/Debug folder
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            ObjectRepository.Config = new AppConfigReader();
            SetupExtentReports();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _featureContext = featureContext;
            if (_featureContext != null)
            {
                _feature = _extentReports.CreateTest<Feature>(_featureContext.FeatureInfo.Title, _featureContext.FeatureInfo.Description);
            }
        }

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {

            _scenarioContext = scenarioContext;
            if (_scenarioContext != null)
            {
                _scenario = _feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title, _scenarioContext.ScenarioInfo.Description);
            }

            //only initialise drive if running web tag or all tag
            var tags = _scenarioContext.ScenarioInfo.Tags;
            if (tags.Contains("web")|| tags.Contains("all"))
            {
                InitWebdriver();
            }
        }

        [AfterStep]
        public static void AfterEachStep()
        {
            ScenarioBlock scenarioBlock = _scenarioContext.CurrentScenarioBlock;

            switch (scenarioBlock)
            {

                case ScenarioBlock.Given:
                    CreateNode<Given>();
                    break;
                case ScenarioBlock.When:
                    CreateNode<When>();
                    break;
                case ScenarioBlock.Then:
                    CreateNode<Then>();
                    break;
                default:
                    CreateNode<And>();
                    break;
            }
        }

        private static void CreateNode<T>() where T : IGherkinFormatterModel
        {
            if (_scenarioContext.TestError != null)
            {
                string name = _scenarioContext.ScenarioInfo.Title.Replace(" ", "") + ".jpeg";
                GenericHelper.TakeScreenShotAsJpeg(name);
                _scenario.CreateNode<T>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message + "\n" + _scenarioContext.TestError.StackTrace, MediaEntityBuilder.CreateScreenCaptureFromPath(name).Build());
            }
            else
            {
                _scenario.CreateNode<T>(_scenarioContext.StepContext.StepInfo.Text).Pass("");
            }

        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Console.WriteLine("Scenario feature: " + _scenarioContext.ScenarioInfo.Title);

            if (_scenarioContext.TestError != null)
            {
                GenericHelper.TakeScreenShotAsJpeg();
            }
            if (ObjectRepository.Driver != null)
            {
                ObjectRepository.Driver.Close();
            }
            
        }

        [AfterTestRun]
        public static void TearDown()
        {
            _extentReports.Flush();
            if (ObjectRepository.Driver != null)
            {
                ObjectRepository.Driver.Close();
                ObjectRepository.Driver.Quit();
            }
        }


        private static void InitWebdriver()
        {
            ObjectRepository.Config = new AppConfigReader();
            switch (ObjectRepository.Config.GetBrowser())
            {
                case BrowserType.Firefox:
                    ObjectRepository.Driver = GetFirefoxDriver();
                    break;
                case BrowserType.Chrome:
                    ObjectRepository.Driver = GetChromeDriver();
                    break;
                default:
                    throw new NoSuitableDriverFoundException($"Driver not found: {ObjectRepository.Config.GetBrowser()}");
            }

            //set defaults
            ObjectRepository.Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ObjectRepository.Config.GetPageLoadTimeOut());
            ObjectRepository.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ObjectRepository.Config.GetElementLoadTimeout());

        }

        private static void SetupExtentReports()
        {
            _extentHtmlReporter = new ExtentHtmlReporter("TestResults");
            _extentHtmlReporter.LoadConfig("extent-config.xml");
            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(_extentHtmlReporter);
        }

        private static IWebDriver GetFirefoxDriver()
        {
            new DriverManager().SetUpDriver(new FirefoxConfig());
            FirefoxDriver driver = new FirefoxDriver();
            return driver;
        }

        private static ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            return options;
        }

        public static IWebDriver GetChromeDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver driver = new ChromeDriver(GetChromeOptions());
            return driver;
        }

    }
}
