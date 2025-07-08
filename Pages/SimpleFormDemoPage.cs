using OpenQA.Selenium;

namespace SeleniumPlaygroundTests.Pages
{
    public class SimpleFormDemoPage : BasePage
    {
        public SimpleFormDemoPage(IWebDriver driver) : base(driver)
        {
        }

        private const string EnterMessageTextboxId = "user-message";
        private const string GetCheckedvalueBtnXpath = "//button[@id = 'showInput']";
        private const string YourMessageId = "message";

        private IWebElement EnterMessageTextbox => Driver.FindElement(By.Id(EnterMessageTextboxId));
        private IWebElement GetCheckedValueButton => Driver.FindElement(By.XPath(GetCheckedvalueBtnXpath));
        private IWebElement YourMessage => Driver.FindElement(By.Id(YourMessageId));

        public SimpleFormDemoPage EnterMessage(string message)
        {
            EnterMessageTextbox.SendKeys(message);
            return this;
        }

        public void ClickGetCheckedValue()
        {
            GetCheckedValueButton.Click();
        }

        public string GetYourMessage()
        {
            return YourMessage.Text;
        }
    }
}
