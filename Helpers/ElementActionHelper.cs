using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPlaygroundTests.Helpers
{
    class ElementActionHelpers
    {
        public static void ClickWhenReady(
        IWebDriver driver,
        By locator,
        TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout)
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException));

            // This will wait until the element is both present & clickable
            IWebElement element = wait.Until(drv =>
            {
                var el = drv.FindElement(locator);
                return (el.Displayed && el.Enabled)
                    ? el
                    : null;
            });

            // Now click—if DOM re-renders under you, catch and retry once
            try
            {
                element.Click();
            }
            catch (StaleElementReferenceException)
            {
                // Re-find and click one more time
                driver.FindElement(locator).Click();
            }

        }
    }
}
