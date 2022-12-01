using OpenQA.Selenium;

namespace selenium.xunit.framework.package.Base
{
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
