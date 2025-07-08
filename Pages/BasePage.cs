using OpenQA.Selenium;

namespace SeleniumPlaygroundTests.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        public string Url => Driver.Url;

        public BasePage(IWebDriver? driver)
        {
            Driver = driver;
        }

        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }
    }
}
