using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Exeptions;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;
using System.Collections.ObjectModel;
using PowerBank_AQA_UITestingCore.Extensions;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Support.Extensions;

namespace PowerBank_AQA_UITestingCore.Models.Providers
{
    public class DriverProvider : IDriverProvider
    {
        public IWebDriver WebDriver { get; set; }

        public Settings.Settings Settings { get; set; }

        public string PageSource => (string)GetPropertyFromDriver(() => WebDriver.PageSource);

        public string Title => (string)GetPropertyFromDriver(() => WebDriver.Title);

        public string Url => (string)GetPropertyFromDriver(() => WebDriver.Url);

        public string CurrentWindowHandle => (string)GetPropertyFromDriver(() => WebDriver.CurrentWindowHandle);

        public int Tabs => (int)GetPropertyFromDriver(() => WebDriver.WindowHandles.Count, 0);

        public ReadOnlyCollection<string> WindowHandles => (ReadOnlyCollection<string>)GetPropertyFromDriver(() => WebDriver.WindowHandles);

        public void CreateDriver(Func<IWebDriver> action, Settings.Settings settings)
        {
            try
            {
                WebDriver = action();
                Settings = settings;
            }
            catch (Exception ex)
            {
                throw new DriverException($"Create driver is return error with message {ex.Message}");
            }
        }

        public IWebDriver GetDriver()
        {
            return WebDriver;
        }

        public void Back()
        {
            try
            {
                WebDriver.Navigate().Back();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Navigate().Back() is return error with message {ex.Message}");
            }
        }

        public void Close()
        {
            try
            {
                WebDriver.Close();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Close() is return error with message {ex.Message}");
            }
        }

        public void Forward()
        {
            try
            {
                WebDriver.Navigate().Forward();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Navigate().Forward() is return error with message {ex.Message}");
            }
        }

        public IElementProvider GetElement(string locator, How how)
        {
            var by = how.GetBy(locator);

            var element = WebDriver.FindElement(@by);
            return new ElementProvider(Settings.Timeout, by)
            {
                WebElement = element,
                WebDriver = WebDriver
            };
        }

        public IEnumerable<IElementProvider> GetElements(string locator, How how)
        {
            var by = how.GetBy(locator);

            var defaultWait = new DefaultWait<IWebDriver>(WebDriver)
            {
                Timeout = TimeSpan.FromSeconds(Settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            var elements = defaultWait.Until(drv => drv.FindElements(@by));
            var listElement = elements.Select(element => new ElementProvider(Settings.Timeout, by) { WebElement = element, WebDriver = WebDriver }).Cast<IElementProvider>().ToList();
            return listElement;
        }

        public void SwitchTo(int number)
        {
            try
            {
                Log.Logger().LogInformation($"SwitchTo().Window to number");
                WebDriver.SwitchTo().Window(WebDriver.WindowHandles[number]);
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Window is return error with message {ex.Message}");
            }
        }

        public IAlertProvider GetAlert()
        {
            try
            {
                var alert = WebDriver.SwitchTo().Alert();
                return new AlertProvider()
                {
                    Alert = alert
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"Switch().Alert is return error with message {ex.Message}");
            }
        }

        public IDriverProvider GetDefaultFrame()
        {
            try
            {
                var driver = WebDriver.SwitchTo().DefaultContent();
                ReadyStateComplete(driver);
                return new DriverProvider()
                {
                    WebDriver = driver,
                    Settings = Settings
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().DefaultContent is return error with message {ex.Message}");
            }
        }

        public IDriverProvider GetParentFrame()
        {
            try
            {
                var driver = WebDriver.SwitchTo().ParentFrame();

                ReadyStateComplete(driver);
                return new DriverProvider()
                {
                    WebDriver = driver,
                    Settings = Settings
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().ParentFrame is return error with message {ex.Message}");
            }
        }

        public IDriverProvider GetFrame(int id)
        {
            try
            {
                Log.Logger().LogDebug($"SwitchTo().Frame by id \"{id}\"");
                var driver = WebDriver.SwitchTo().Frame(id);
                ReadyStateComplete(driver);
                return new DriverProvider()
                {
                    WebDriver = driver,
                    Settings = Settings
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Frame by id \"{id}\" is return error with message {ex.Message}");
            }
        }

        public IDriverProvider GetFrame(string name)
        {
            try
            {
                Log.Logger().LogDebug($"SwitchTo().Frame by name \"{name}\"");

                ReadyStateComplete(WebDriver);
                var driver = WebDriver.SwitchTo().Frame(name);

                ReadyStateComplete(driver);
                return new DriverProvider
                {
                    WebDriver = driver,
                    Settings = Settings
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Frame by name \"{name}\" is return error with message {ex.Message}");
            }
        }

        public IDriverProvider GetFrame(By by)
        {
            try
            {
                Log.Logger().LogDebug($"SwitchTo().Frame by locator");
                var element = WebDriver.FindElement(by);
                var driver = WebDriver.SwitchTo().Frame(element);

                ReadyStateComplete(driver);
                return new DriverProvider()
                {
                    WebDriver = driver,
                    Settings = Settings
                };
            }
            catch (Exception ex)
            {
                throw new DriverException($"SwitchTo().Frame by locator is return error with message {ex.Message}");
            }
        }

        public void GoToUrl(string url)
        {
            try
            {
                Log.Logger().LogDebug($"Go to \"{url}\"");
                GoToUrl(WebDriver, url);
            }
            catch (Exception ex)
            {
                throw new DriverException($"Go to \"{url}\" is return error with message {ex.Message}");
            }
        }

        public void Maximize()
        {
            try
            {
                WebDriver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Manage().Window.Maximize is return error with message {ex.Message}");
            }
        }

        public void Quit()
        {
            try
            {
                WebDriver.Quit();
                WebDriver = null;
            }
            catch (Exception ex)
            {
                throw new DriverException($"Quit browser is return error with message {ex.Message}");
            }
        }

        public void Refresh()
        {
            try
            {
                WebDriver.Navigate().Refresh();
            }
            catch (Exception ex)
            {
                throw new DriverException($"Navigate().Refresh is return error with message {ex.Message}");
            }
        }

        public byte[] Screenshot()
        {
            //var screenshotMaker = new ScreenshotMaker();
            //screenshotMaker.RemoveScrollBarsWhileShooting();
            //return WebDriver.TakeScreenshot(screenshotMaker);
            throw new NotImplementedException();
        }

        public void WindowSize(int width, int height)
        {
            try
            {
                Log.Logger().LogDebug($"Set browser window size as ({width},{height})");
                WebDriver.Manage().Window.Size = new System.Drawing.Size(width, height);
            }
            catch (Exception ex)
            {
                throw new DriverException($"Manage().Window.Size as ({width},{height}) is return error with message {ex.Message}");
            }
        }

        private void GoToUrl(IWebDriver driver, string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                driver.Navigate().GoToUrl(new Uri(url));
            }
            else
            {
                driver.Navigate().GoToUrl(url);
            }

            Log.Logger().LogDebug($"Go to {url}");

            Log.Logger().LogInformation($"Driver wait page ready state completed");
            ReadyStateComplete(driver);
        }

        private void ReadyStateComplete(IWebDriver driver)
        {
            var driverWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(Settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };
            driverWait.Until(webDriver => ((IJavaScriptExecutor)webDriver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        private object GetPropertyFromDriver(Func<object> func, object defaultValue = null)
        {
            try
            {
                return func();
            }
            catch (WebDriverException e)
            {
                Log.Logger().LogWarning(e.Message);
                return defaultValue;
            }
        }
    }
}
