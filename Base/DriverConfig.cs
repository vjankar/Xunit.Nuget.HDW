using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace selenium.xunit.framework.package.Base
{
    public class DriverConfig
    {
        public IWebDriver Driver { get; set; }

        public MediaEntityModelProvider CaptureScreenshotAndReturnModel(string Name)
        {
            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;

            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, Name).Build();
        }
    }
}
