using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System.Net.Http.Headers;
using System.Text;

namespace SeleniumPlaygroundTests.WebDrivers
{
    public static class WebDriverUtils
    {
        public static IWebDriver LaunchWebDriver(
            string browser,
            string browserVersion,
            string platform)
        {
            // fetch your LT creds
            string _userName = GetSystemVariable("LT_USERNAME");
            string _accessKey = GetSystemVariable("LT_ACCESS_KEY");
            if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_accessKey))
                throw new InvalidOperationException("Set LT_USERNAME & LT_ACCESS_KEY first.");

            DriverOptions options = browser.ToLower() switch
            {
                "chrome" => new ChromeOptions(),
                "firefox" => new FirefoxOptions(),
                "edge" => new EdgeOptions(),
                "safari" => new SafariOptions(),
                _ => throw new ArgumentException($"Unsupported browser: {browser}")
            };

            options.BrowserVersion = browserVersion;
            var ltOpt = new Dictionary<string, object>
            {
                ["username"] = _userName,
                ["accessKey"] = _accessKey,
                ["platformName"] = platform,
                ["project"] = "Web",
                ["build"] = "Selenium Playground Parallel Build",
                ["name"] = $"{browser} on {platform}",
                ["console"] = "info",
                ["w3c"] = true,
                ["plugin"] = "c#-nunit"
            };
            if (browser.Equals("safari", StringComparison.OrdinalIgnoreCase))
            {
                ltOpt["automaticInspection"] = true;
            }
            options.AddAdditionalOption("LT:Options", ltOpt);

            var hub = new Uri("https://hub.lambdatest.com/wd/hub");
            var driver = new RemoteWebDriver(hub, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            driver.Manage().Window.Maximize();
            return driver;
        }

        private static string GetSystemVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
            //var value = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User) ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine); return value;
        }


        public static void MarkTestStatus(IWebDriver driver, bool status, string reason = "")
        {
            try
            {

                ((IJavaScriptExecutor)driver).ExecuteScript($"lambda-status={status};");

               /* if (!string.IsNullOrEmpty(reason))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("lambda-comment=arguments[0];", reason);
                }
*/
                // Optional: give LambdaTest time to register the status
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed to mark LambdaTest status: {ex.Message}");
            }
        }


        public static void UpdateLambdaTestStatus(IWebDriver driver, bool passed, string reason = "")
        {
            try
            {
                var sessionId = ((RemoteWebDriver)driver).SessionId.ToString();
                var username = GetSystemVariable("LT_USERNAME");
                var accessKey = GetSystemVariable("LT_ACCESS_KEY");
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{accessKey}"));

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                    var status = passed ? "passed" : "failed";
                    var content = new StringContent(
                    $"{{\"status_ind\":\"{status}\",\"reason\":\"{reason}\"}}",
                    Encoding.UTF8,
                    "application/json"
                    );

                    var response = client.PutAsync(
                    $"https://api.lambdatest.com/automation/api/v1/sessions/{sessionId}",
                    content
                    ).Result;

                    var result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"LambdaTest API response: {result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update LambdaTest status: {ex.Message}");
            }
        }





    }
}