using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPlaygroundTests.WebDrivers
{
    public static class WebDriverExtensions
    {
        public static IWebElement WaitForElement(this IWebDriver driver, By locator, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            return wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(locator);
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }
    }
}