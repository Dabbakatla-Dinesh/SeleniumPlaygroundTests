using OpenQA.Selenium;
using SeleniumPlaygroundTests.Config;
using SeleniumPlaygroundTests.Pages;
using SeleniumPlaygroundTests.WebDrivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumPlaygroundTests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class InputFormSubmitTests
    {
        private IWebDriver driver;
        private string browser;
        private string version;
        private string platform;

        [SetUp]
        public void SetUp()
        {
            driver = null;
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }

        [Test, TestCaseSource(typeof(BrowserConfigs), nameof(BrowserConfigs.All))]
        public void ValidateInputFormSubmit(string browser, string version, string platform)
        {
            this.browser = browser;
            this.version = version;
            this.platform = platform;
            var urlKey = "SeleniumPlayground";

            try
            {
                driver = WebDriverUtils.LaunchWebDriver(browser, version, platform);

                var playgroundPage = new SeleniumPlaygroundPage(driver);
                playgroundPage.NavigateTo(ConfigReader.GetUrl(urlKey));

                string name = "John Doe";
                string email = "Example@email.com";
                string password = "Password123";
                string company = "LambdaTest";
                string address1 = "123 Main St";
                string address2 = "Suite 100";
                string websiteName = "www.example.com";
                string country = "United States";
                string state = "California";
                string city = "Los Angeles";
                string zip = "90001";
                string expectedValidatedMessage = browser.StartsWith("safari", StringComparison.OrdinalIgnoreCase)
                    ? "Fill out this field"
                    : "Please fill out this field.";
                string expectedSuccessMessage = "Thanks for contacting us, we will get back to you shortly.";

                var inputFormSubmitPage = playgroundPage.ClickInputFormSubmit();
                var validationMessage = inputFormSubmitPage.DoesValidationMessageDisplayed();

                inputFormSubmitPage.FillForm(name, email, password, company, country, address1, address2, websiteName, state, city, zip);
                inputFormSubmitPage.ClickSubmitButton();

                var successMessage = inputFormSubmitPage.GetSuccessMessage;
                bool isSuccessMessageDisplayed = successMessage.Equals(expectedSuccessMessage);
                bool isValidationMessageDisplayed = validationMessage.Equals(expectedValidatedMessage);

                WebDriverUtils.MarkTestStatus(driver, isSuccessMessageDisplayed && isValidationMessageDisplayed);

                Assert.That(successMessage, Is.EqualTo(expectedSuccessMessage), "The success message did not match the expected text.");
                Assert.That(validationMessage, Is.EqualTo(expectedValidatedMessage), $"The validation message did not match the expected text.");
            }
            catch (Exception ex)
            {
                WebDriverUtils.MarkTestStatus(driver, false, ex.Message);
                throw;
            }
        }
    }
}
