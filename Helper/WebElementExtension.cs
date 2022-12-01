using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace selenium.xunit.framework.package.Helper
{
    public static class WebElementExtensions
    {
        public static void SetAttribute(this IWebElement element, string attributeName, string value)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            if (wrappedElement == null)
                throw new ArgumentException("element", "Element must wrap a web driver");

            IWebDriver driver = wrappedElement.WrappedDriver;
            IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("element",
                    "Element must wrap a web driver that supports javascript execution");

            javascript.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", element, attributeName, value);
        }

        public static void RemoveAttribute(this IWebElement element, string attributeName)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            if (wrappedElement == null)
                throw new ArgumentException("element", "Element must wrap a web driver");

            IWebDriver driver = wrappedElement.WrappedDriver;
            IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("element",
                    "Element must wrap a web driver that supports javascript execution");

            javascript.ExecuteScript("arguments[0].removeAttribute(arguments[1], arguments[2])", element, attributeName);
        }

        public static void RemoveAttribute(this ReadOnlyCollection<IWebElement> element, string attributeName, string attributeValue)
        {
            foreach (var e in element)
            {
                IWrapsDriver wrappedElement = e as IWrapsDriver;
                if (wrappedElement == null)
                    throw new ArgumentException("element", "Element must wrap a web driver");

                IWebDriver driver = wrappedElement.WrappedDriver;
                IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
                if (javascript == null)
                    throw new ArgumentException("element",
                        "Element must wrap a web driver that supports javascript execution");

                javascript.ExecuteScript("arguments[0].removeAttribute(arguments[1], arguments[2])", e, attributeName, attributeValue);
            }
        }

        public static void SendKeys(this IWebElement element, string value, bool clearFirst = false, bool useJavaScriptExecutor = false)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            IWebDriver driver = wrappedElement.WrappedDriver;

            if (useJavaScriptExecutor)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value=arguments[1]", element, value);
            }
            else
            {
                if (clearFirst) element.Clear();
                element.SendKeys(value);
            }

            driver.ScriptExecute("arguments[0].blur();", element);
        }

        public static void SetCheckBox(this IWebElement element, bool state)
        {
            if (state == true)
            {
                if (element.GetAttribute("checked") == "checked" || element.GetAttribute("value") == "true" ||
                    element.GetAttribute("checked") == "true")
                {
                    Console.WriteLine("...checkbox already in correct state");
                }
                else
                {
                    element.Click();
                }
            }

            if (state == false)
            {
                if (element.GetAttribute("checked") == null || element.GetAttribute("value") == "false" ||
                    element.GetAttribute("checked") == "false")
                {
                    Console.WriteLine("...checkbox already in correct state");
                }
                else
                {
                    element.Click();
                }
            }
        }

        public static void MultiselectDropDownAdd(this IWebElement element, IWebDriver driver, string[] select)
        {
            foreach (var s in select)
            {
                element.Click();
                Thread.Sleep(1000); //TODO: Fix this...and won't select from dropdown item that is not visible.
                var selectElement = driver.Find(By.XPath($"//li[text()='{s}']"));
                selectElement.Click();
            }
        }

        public static void SelectDropdownValue(this IWebElement element, string selectText = null)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            IWebDriver driver = wrappedElement.WrappedDriver;

            var selectElement = new SelectElement(element);

            WaitTime.WaitForResult(() => true == selectElement.Options.Count > 0);

            if (selectText != null)
                selectElement.SelectByText(selectText);
            else
                selectElement.SelectByIndex(1);

            driver.ScriptExecute("arguments[0].blur()", selectElement);
        }

    }
}
