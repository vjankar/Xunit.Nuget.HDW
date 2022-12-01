using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using selenium.xunit.framework.package.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TechTalk.SpecFlow;

namespace selenium.xunit.framework.package.Base
{
    public class TestBase : Steps
    {
        private readonly DriverConfig Selenium;

        public TestBase(DriverConfig driver)
        {
            Selenium = driver;
        }

        public void InitializeSettings()
        {
            //Set App settings
            ConfigReader.SetAppSettings();

            Selenium.Driver = GetWebDriver();
            Selenium.Driver.Manage().Window.Maximize();
        }

        private IWebDriver GetWebDriver()
        {
            var driverPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                                                           + @"\..\..\..\bin\debug\netcoreapp3.1"));
            //var driverPath = new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

            var selectedBrowser = AppConfig.WebDriver;
            Console.WriteLine("...Opening {0}", selectedBrowser);
            switch (selectedBrowser)
            {
                case "firefox":
                    var service = FirefoxDriverService.CreateDefaultService(driverPath);
                    service.FirefoxBinaryPath = @"C:\Program Files(x86)\Mozilla Firefox\firefox.exe";
                    return new FirefoxDriver(service);
                case "ie":
                    var options = new InternetExplorerOptions
                    {
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        EnsureCleanSession = true,
                        EnableNativeEvents = true,
                        UnhandledPromptBehavior = UnhandledPromptBehavior.Accept,
                        BrowserCommandLineArguments = "-private",
                        RequireWindowFocus = true //Required for it to run propertly maybe
                    };
                    return new InternetExplorerDriver(driverPath, options);
                case "edge":
                    var timeOutTimeEdge = TimeSpan.FromMinutes(5);
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AcceptInsecureCertificates = true;
                    return new EdgeDriver(driverPath, edgeOptions, timeOutTimeEdge);
                case "chrome":
                    var timeOutTime = TimeSpan.FromMinutes(5);
                    ChromeOptions chromeOptions = new ChromeOptions();

                    chromeOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                    chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                    chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                    chromeOptions.AddUserProfilePreference("download.prompt_for_download", "false");
                    chromeOptions.AddUserProfilePreference("yourprotocolname", "false");
                    chromeOptions.AddUserProfilePreference("ExternalProtocolDialogShowAlwaysOpenCheckbox", "true");

                    chromeOptions.AddArguments("--disable-gpu");
                    chromeOptions.AddArguments("--no-sandbox");
                    chromeOptions.AddArguments("--allow-insecure-localhost");
                    chromeOptions.AddArguments("--enable-extensions");
                    chromeOptions.AddArguments("test-type");
                    chromeOptions.AddArguments("window-size=1920,1080");
                    chromeOptions.AddArguments("--disable-web-security");
                    chromeOptions.AddArguments("--allow-running-insecure-content");
                    chromeOptions.AddArguments("--ignore-certificate-errors");
                    chromeOptions.AddArguments("use-fake-ui-for-media-stream");
                    //chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    chromeOptions.AcceptInsecureCertificates = true;
                    //chromeOptions.AddAdditionalCapability("acceptInsecureCerts", true, true);
                    //chromeOptions.AddAdditionalChromeOption("acceptInsecureCerts", true);
                    //new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    return new ChromeDriver(driverPath, chromeOptions, timeOutTime);
                case "chromeHeadless":
                    var timeOut = TimeSpan.FromMinutes(5);
                    ChromeOptions cOptions = new ChromeOptions();

                    cOptions.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                    cOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                    cOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                    cOptions.AddUserProfilePreference("download.prompt_for_download", "false");
                    cOptions.AddUserProfilePreference("yourprotocolname", "false");
                    cOptions.AddUserProfilePreference("ExternalProtocolDialogShowAlwaysOpenCheckbox", "true");

                    cOptions.AddArguments("--headless");
                    cOptions.AddArguments("--disable-gpu");
                    cOptions.AddArguments("--no-sandbox");
                    cOptions.AddArguments("--allow-insecure-localhost");
                    cOptions.AddArguments("--enable-extensions");
                    cOptions.AddArguments("test-type");
                    cOptions.AddArguments("window-size=1920,1080");
                    cOptions.AddArguments("--disable-web-security");
                    cOptions.AddArguments("--allow-running-insecure-content");
                    cOptions.AddArguments("--ignore-certificate-errors");
                    cOptions.AddArguments("use-fake-ui-for-media-stream");
                    //cOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    //cOptions.AddAdditionalCapability("acceptInsecureCerts", true, true);
                    cOptions.AcceptInsecureCertificates = true;
                    //new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    return new ChromeDriver(driverPath, cOptions, timeOut);
                default:
                    throw new Exception($"...Web driver not found: {selectedBrowser}");
            }
        }
    }
}
