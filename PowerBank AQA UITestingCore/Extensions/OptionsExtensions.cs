using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PowerBank_AQA_TestingCore.Helpers;

namespace PowerBank_AQA_UITestingCore.Extensions
{
    public static class OptionsExtensions
    {
        public static DriverOptions AddCapabilities(this DriverOptions driverOptions, Dictionary<string, string> capabilities)
        {
            var _options = driverOptions;

            if(capabilities is null || !capabilities.Any())
            {
                Log.Logger().LogInformation($"Коллецкия capabilities браузера null или пуста. {driverOptions.GetType().Name.ToLower()} созданы без дополнительных capabilities");
                return null;
            }

            foreach (var (key, value) in capabilities)
            {
                _options.AddAdditionalOption(key, value);
            }

            return _options;
        }

        public static DriverOptions AddUserProfilePreferences(this ChromeOptions driverOptions, Dictionary<string, string> userProfilePreferences)
        {
            var _options = driverOptions;

            if (userProfilePreferences is null || !userProfilePreferences.Any())
            {
                Log.Logger().LogInformation($"Коллецкия UserProfilePreferences браузера null или пуста. {driverOptions.GetType().Name.ToLower()} созданы без UserProfilePreferences");
                return null;
            }

            foreach (var (key, value) in userProfilePreferences)
            {
                _options.AddAdditionalOption(key, value);
            }

            return _options;
        }
    }
}
