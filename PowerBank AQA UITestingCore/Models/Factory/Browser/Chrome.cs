using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_TestingCore.Models;
using PowerBank_AQA_UITestingCore.Extensions;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.PageObject;

namespace PowerBank_AQA_UITestingCore.Models.Factory.Browser
{
    public class Chrome : Browser
    {
        public Chrome(Settings.Settings settings, IEnumerable<Node> pages)
        {
            Settings = settings;
            Pages = pages;

            var options = CreateOptions(settings);
            var timeout = Settings.Timeout;

            Log.Logger().LogInformation($"Запуск браузера Chrome");
            var service = settings.IsDriverPath() ? ChromeDriverService.CreateDefaultService(settings.DriverLocation) 
                : ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            DriverProvider.CreateDriver(() => new ChromeDriver(service, options, TimeSpan.FromSeconds(timeout)), settings);
            SessionId = (DriverProvider.GetDriver() as ChromeDriver)?.SessionId;
            Log.Logger().LogInformation($"Chrome Browser стартовал локально, SessionID: {SessionId}");
            Settings = settings;
        }

        public sealed override SessionId SessionId { get; protected set; }

        private ChromeOptions CreateOptions(Settings.Settings settings)
        {
            var options = new ChromeOptions();

            if(settings.IsExtensions())
            {
                var list = new List<string>();
                var binDir = new BinDirectory();

                foreach(var extension in Settings.Extensions)
                {
                    string path = null;
                    var fileExt = Path.GetExtension(extension);
                    if(fileExt is Constants.EXTENSION_EXT)
                    {
                        path = binDir.Get() + "\\" + extension;
                    }
                    path ??= binDir.Get() + "\\" + extension + Constants.EXTENSION_EXT;

                    if(File.Exists(path))
                    {
                        Log.Logger().LogInformation($"Добавлено расширение {extension}");
                        list.Add(path);
                    }
                    else
                    {
                        Log.Logger().LogInformation($"Расширение {extension} не существует в bin директории");
                    }
                }

                options.AddExtensions(list);
            }

            if(settings.IsOptions())
            {
                options.AddArguments(Settings.Options);
            }

            if(!settings.IsBinaryPath())
            {
                options.BinaryLocation = settings.BinaryLocation;
            }

            if(settings.CheckCapability())
            {
                options.AddCapabilities(settings.Capabilities);
            }

            if(settings.CheckUserProfilePreference())
            {
                options.AddUserProfilePreferences(settings.UserProfilePreference);
            }

            options.SetLoggingPreference(LogType.Browser, OpenQA.Selenium.LogLevel.Debug);
            return options;
        }
    }
}
