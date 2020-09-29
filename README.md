
# Ding_Tech_Challenge

## Demo Specflow/Selenium Framework for Paypal website and PetStore API

Fully tested using Chrome and Firefox

## PreReq

Chrome/Firefox installed

Either Visual Studio 2019 installed
OR
MSBuild tools installed
Nuget.exe on path


## Execution

Visual Studio 2019

Clone repo into visual studio. VS should have Specflow and NUnit extension installed. Build solution, open test explorer and select tests

OR

MSBuild/Nuget

1. Open Repo in folder explorer

    nuget restore TechChallenge.sln
	
This will add required packages

2.   [path to]\msbuild.exe [path to]\TechChallenge.sln
   
This will create TechChallenge.dll in bin/debug folder
  
3. Nunit console runner is in Packages\NUnit.ConsoleRunner.3.11.1\tools. Either open cmd line here, or copy path and

    [path to]nunit3-console.exe [path to]\TechChallenge.dll
	
Will run all web tests on default browser (Chrome)

To specify browser add following switch

    --testparam:BROWSER="Firefox"

To run selected tests by tags

     --where cat="all"

   
List of tags: 
* @web                       runs only paypal selenium tests
* @api							runs only API tests 
* @all                         runs all tests - web and api


Framework is Specflow based and uses page object with page factory to keep code clean. DriverManager ensures that latest compatible WebDriver is downloaded.
Reporting is done by Extent reports, logging from log4net and asserts from fluentAssert for readability


BaseDefinition class is where all the Specflow Hooks to initialise tests are, including initialising the WebDriver and setting up Extent reporting.

Extent Reports - index.html file will be created in bin/debug folder. Screenshots taken on failure for web based tests

## Description of namespaces


### ComponentHelper
Wrapper class for common IWebElements and JavaScript executor. Contains logging.

### Configuration
Browser choice Enum and Class to get values from App.config file

### CustomExceptions
Extends the Exception class

### FeatureFiles
Specflow features

### Interfaces
IConfig - used in initialisation

### JSONFiles
Json file for creating a pet via API

### Logging
Retrieves xml logging setup info from App.config. Appended log files written to bin/debug

### PageObject
All pages used, base page creates instance of logger

### POCO
Data objects for API

### Settings
ObjectRepository holds driver instance and congig instance(driver timeout etc)

### StepDefinition
Defined from feature files

### Text
Any text used
