using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace selenium.xunit.framework.package.Helper
{
    public static class WebDriverExtensions
    {
        public static string ParentWindowHandler = string.Empty;
        public static string CurrentFrameName = string.Empty;

        public static void MoveMouseTo(this IWebDriver driver, IWebElement element)
        {
            var builder = new Actions(driver);
            builder.MoveToElement(element).Build().Perform();
        }

        public static void WaitForUrlToContain(this IWebDriver driver, string urlPart, TimeSpan? timeoutPeriodOverride = null)
        {
            var sw = Stopwatch.StartNew();
            var timeoutPeriod = TimeSpan.FromSeconds(120);
            if (timeoutPeriodOverride.HasValue)
            {
                timeoutPeriod = timeoutPeriodOverride.Value;
            }
            while (!driver.Url.ToLowerInvariant().Contains(urlPart.ToLowerInvariant()) && sw.Elapsed < timeoutPeriod)
            {
                Console.WriteLine("...Waiting for '{2}' to be in URL - Current URL: {0} (Time elapsed: {1})", driver.Url, sw.Elapsed, urlPart);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            sw.Stop();

            if (!driver.Url.ToLowerInvariant().Contains(urlPart.ToLowerInvariant()))
            {
                throw new TimeoutException($"...Timeout waiting to be redirected to a page that contains url: {urlPart}");
            }
            else
            {
                Console.WriteLine("...Page loaded in: {0}.  URL: {1}", sw.Elapsed, driver.Url);
            }
        }

        public static IWebElement Find(this IWebDriver driver, By by, ExpectedCondition condition = ExpectedCondition.Visible, int waitTime = 15)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException), typeof(NoSuchElementException));

            switch (condition)
            {
                case ExpectedCondition.Clickable:
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(by));
                        break;
                    }
                case ExpectedCondition.Visible:
                    {
                        wait.Until(ExpectedConditions.ElementIsVisible(by));
                        break;
                    }
                case ExpectedCondition.SwitchToFrame:
                    {
                        wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(by));
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
            }

            return driver.FindElement(by);
        }

        public static ReadOnlyCollection<IWebElement> Finds(this IWebDriver driver, By by)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException), typeof(NoSuchElementException));
            wait.Until(ExpectedConditions.ElementIsVisible(by));

            return driver.FindElements(by);
        }

        public static void ClickNavigationLink(this IWebDriver driver, By hoverTab, By subLink, By subSubLink = null)
        {
            Console.WriteLine("...If the navigation fails, move the cucrsor above the webpage.  Any movement will upset the HoverOver menu.");
            var action = new Actions(driver);
            action.MoveToElement(driver.Find(hoverTab)).Perform();
            Thread.Sleep(1000);
            if (subLink != null && subSubLink == null)
            {
                driver.Find(subLink).Click();
            }
            else
            {
                action.MoveToElement(driver.Find(subLink)).Perform();
                Thread.Sleep(500);
                driver.Find(subSubLink).Click();
            }
        }

        public static IWebElement ScrollToElement(this IWebDriver driver, IWebElement element)
        {
            return (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public static void ScrollToBottom(this IWebDriver driver)
        {
            long scrollHeight = 0;

            do
            {
                var js = (IJavaScriptExecutor)driver;
                var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");

                if (newScrollHeight == scrollHeight)
                {
                    break;
                }
                else
                {
                    scrollHeight = newScrollHeight;
                    Thread.Sleep(400);
                }
            } while (true);
        }

        public static bool IsElementOnPage(this IWebDriver driver, By locator)
        {
            driver.LowerImplicitWaitTimeoutForNegativeAssertions();
            var e = driver.FindElements(locator);
            var count = e.Count;
            driver.SetTimeoutsToDefaults();

            return count > 0;
        }

        public static void SetTimeoutsToDefaults(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(90);
        }

        public static void ScriptExecute(this IWebDriver driver, string script, params object[] args)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(script, args);
        }

        public static void LowerImplicitWaitTimeoutForNegativeAssertions(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(0);
        }

        public static void SwitchToIframe(this IWebDriver driver, By elementLocator, int timeout = 30)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(elementLocator));
                var frame = driver.FindElement(elementLocator);
                driver.SwitchTo().Frame(frame);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        public static void SwitchToNewWindow(this IWebDriver driver)
        {
            var newWindowIndex = driver.WindowHandles.Count - 1;
            driver.SwitchTo().Window(driver.WindowHandles[newWindowIndex]);
            driver.Manage().Window.Maximize();
        }

        public static void PageSourceWait(this IWebDriver Driver)
        {
            IWait<IWebDriver> wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, TimeSpan.FromSeconds(60.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)Driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void SwitchToWindow(this IWebDriver Driver, string mainhandle)
        {
            string popupHandle = string.Empty;
            ReadOnlyCollection<string> windowHandles = Driver.WindowHandles;

            foreach (string handle in windowHandles)
            {
                if (handle != mainhandle)
                {
                    popupHandle = handle; break;
                }
            }
            //switch to new window 
            Driver.SwitchTo().Window(popupHandle);
        }

        public static IWebDriver SwitchToPopUpWindow(this IWebDriver Driver)
        {
            ParentWindowHandler = Driver.CurrentWindowHandle;
            //string subWindowHandler = null;
            List<string> handles = Driver.WindowHandles.ToList(); // get all window handles

            //Instance.SwitchTo().Window(handles.Last()).Manage().Cookies.DeleteAllCookies();
            return Driver.SwitchTo().Window(handles.Last());
            //return Driver.Instance;
        }

        public static void SwitchToMainWindow(this IWebDriver Driver)
        {
            CurrentFrameName = "";
            if (string.IsNullOrEmpty(ParentWindowHandler))
            {
                ParentWindowHandler = Driver.WindowHandles.First();
            }
            Driver.SwitchTo().Window(ParentWindowHandler);
        }

        // Clicks context menu in Grid based on given row number and column number
        public static void ContextMenuClickusingColumnNumber(this IWebDriver driver, int rowNumber, int colNumber, string itemName)
        {
            IWebElement row = null;
            try
            {
                row = GetRow(driver, rowNumber);
                var tdlist = row.FindElements(By.TagName("td"));
                var divCtxMenu = tdlist[colNumber].FindElement(By.ClassName("action-menu"));
                var lists = divCtxMenu.FindElement(By.TagName("div")).FindElement(By.TagName("ul")).FindElements(By.TagName("li"));

                foreach (var list in lists)
                {
                    if (list.Text.Contains(itemName))
                    {
                        WaitTime.Wait(2);
                        list.Click();
                        WaitTime.Wait(2);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("GetRow returned null row");
            }
        }

        // Gets the cell value from the grid	
        public static string GetCellValue(this IWebDriver driver, int rowNo, int colNo)
        {
            string cellValue = "";
            int count = 0;

            IWebElement row = GetRow(driver, rowNo);
            var tds = row.FindElements(By.TagName("td"));
            foreach (var td in tds)
            {
                if (count == colNo)
                {
                    cellValue = td.Text;
                    break;
                }
                count++;
            }
            return cellValue;
        }

        // Fetches row element in the grid
        public static IWebElement GetRow(this IWebDriver driver, int rowNo)
        {
            IWebElement grid;
            IWebElement row = null;

            try
            {
                grid = driver.FindElement(By.ClassName("rgMasterTable"));
                System.Collections.Generic.List<IWebElement> gridRows = grid.FindElements(By.TagName("tr")).ToList();
                row = gridRows.Where(r => r.GetAttribute("class").Contains("Row")).First(x => x.GetAttribute("id").Split('_').Last().Equals(rowNo - 1 + ""));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in finding Grid in the Page: " + ex.Message + "\n" + ex.StackTrace);
            }

            return row;
        }
    }

    public enum ExpectedCondition
    {
        Clickable,
        Visible,
        SwitchToFrame
    }
}
