using Microsoft.Extensions.Configuration;

namespace SeleniumPlaygroundTests.Config
{
    public static class ConfigReader
    {
        private static IConfigurationRoot config;

        static ConfigReader()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string GetUrl(string key) => config[$"TestUrls:{key}"];
    }
}
