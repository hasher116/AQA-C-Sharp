using BoDi;
using PowerBank_AQA_UITestingCore.Models.Settings;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.Factory.Browser;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using Bogus.DataSets;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;


namespace PowerBank_AQA_SpecFlow.StepDefinitions
{
    public class BrowserStepDefinitions
    {
        private readonly Settings _settings;
        private IObjectContainer _container;
        private readonly IEnumerable<Node> _pages;
        public BrowserStepDefinitions(Settings settings, IObjectContainer container, IEnumerable<Node> pages)
        {
            _settings = settings;
            _pages = pages;
            _container = container;
        }

        private IBrowser Create(Settings settings, IEnumerable<Node> pages)
        {
            switch (settings.Browser)
            {
                case BrowserType.CHROME:
                {
                    var browser = new Chrome(settings, pages);
                    Log.Logger().LogInformation("Старт ChromeBrowser");
                    return browser;
                }
                case BrowserType.FIREFOX:
                {
                    var browser = new Firefox(settings, pages);
                    Log.Logger().LogInformation("Старт FirefoxBrowser");
                    return browser;
                }
                default:
                    throw new InvalidOperationException($"Неизвестный тип браузера {settings.Browser.ToString()}");
            }
        }

        public void StartBrowser()
        {
            Log.Logger().LogInformation("Осуществлен старт бразуера");
            var browser = Create(_settings, _pages);
            _container.RegisterInstanceAs(browser);
        }

        public IBrowser GetBrowser()
        {
            return _container.Resolve<IBrowser>();
        }

        public void GoToPage(string url)
        {
            Log.Logger().LogInformation($"Переход по URL {url}");
            _container.Resolve<IBrowser>().GoToPage(url);
        }

        public void SetCurrentPage(string name)
        {
            Log.Logger().LogInformation($"Установить текущую страницу с локатором {name}");
            _container.Resolve<IBrowser>().SetCurrentPage(name);
        }

        public void UpdateCurrentPage(string name)
        {
            Log.Logger().LogInformation($"Обновить текущую страницу с локатором {name}");
            _container.Resolve<IBrowser>().SetCurrentPage(name, false);
        }

        public void CloseBrowser()
        {
            Log.Logger().LogInformation("Закрыть драйвер браузера");
            _container.Resolve<IBrowser>().Quit();
        }

        public void SwitchTo(int number)
        {
            Log.Logger().LogInformation($"Переключение на вкладку {number}");
            _container.Resolve<IBrowser>().SwitchTo(number);
        }

        public string GetCurrentUrl()
        {
            Log.Logger().LogInformation($"Получен Url текущей страницы:  {_container.Resolve<IBrowser>().Url}");
            return _container.Resolve<IBrowser>().Url;
        }

        public void Close()
        {
            Log.Logger().LogInformation($"Вкладка закрыта");
            _container.Resolve<IBrowser>().Close();
        }
    }
}
