using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPlaygroundTests.Helpers;

namespace SeleniumPlaygroundTests.Pages
{
    public class InputFormSubmitPage : BasePage
    {
        public InputFormSubmitPage(IWebDriver? driver) : base(driver)
        {
        }
        private const string NameId = "name";
        private const string EmailId = "inputEmail4";
        private const string PaswordId = "inputPassword4";
        private const string CompanyId = "company";
        private const string Address1Id = "inputAddress1";
        private const string Address2Id = "inputAddress2";
        private const string WebsiteNamedId = "websitename";
        private const string CountryDropDownName = "country";
        private const string StateId = "inputState";
        private const string CityId = "inputCity";
        private const string ZipId = "inputZip";
        private const string SubmitButtonXpath = "//button[@type='submit' and .='Submit']";
        private const string SucessMessageXpath = "//p[@class='success-msg hidden']";


        private IWebElement Name => Driver.FindElement(By.Id(NameId));
        private IWebElement Email => Driver.FindElement(By.Id(EmailId));
        private IWebElement Password => Driver.FindElement(By.Id(PaswordId));
        private IWebElement Company => Driver.FindElement(By.Id(CompanyId));
        private IWebElement Address1 => Driver.FindElement(By.Id(Address1Id));

        private IWebElement Address2 => Driver.FindElement(By.Id(Address2Id));
        private IWebElement WebsiteName => Driver.FindElement(By.Id(WebsiteNamedId));
        private IWebElement CountryDropDown => Driver.FindElement(By.Name(CountryDropDownName));
        private IWebElement State => Driver.FindElement(By.Id(StateId));
        private IWebElement City => Driver.FindElement(By.Id(CityId));
        private IWebElement Zip => Driver.FindElement(By.Id(ZipId));
        private By SubmitButton => By.XPath(SubmitButtonXpath);

        public string GetSuccessMessage => Driver.FindElement(By.XPath(SucessMessageXpath)).Text;

        public void ClickSubmitButton()
        {
            ElementActionHelpers.ClickWhenReady(Driver, SubmitButton, TimeSpan.FromSeconds(10));
        }

        public InputFormSubmitPage FillForm(string name, string email, string password, string company, string country, string address1, string address2, string websiteName, string state, string city, string zip)
        {
            Name.SendKeys(name);
            Email.SendKeys(email);
            Password.SendKeys(password);
            Company.SendKeys(company);
            Address1.SendKeys(address1);
            Address2.SendKeys(address2);
            WebsiteName.SendKeys(websiteName);
            new SelectElement(CountryDropDown).SelectByText(country);
            State.SendKeys(state);
            City.SendKeys(city);
            Zip.SendKeys(zip);
            return this;
        }

        public string DoesValidationMessageDisplayed()
        {
            try
            {
                ClickSubmitButton();
                Thread.Sleep(2000); // Wait for validation message to appear
                string validationMessage = Name.GetAttribute("validationMessage");
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return validationMessage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }


    }
}
