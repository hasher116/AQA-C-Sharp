using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using PowerBank_AQA_TestingCore.Helpers;

namespace PowerBank_AQA_UITestingCore.Helpers
{
    public static class Message
    {
        public static string CreateMessage(this DriverOptions driverOptions)
        {
            if (driverOptions is { })
            {
                return $@"{driverOptions}";
            }

            Log.Logger().LogInformation($"Строка создания DriverOptions пустая");
            return string.Empty;
        }
    }
}
