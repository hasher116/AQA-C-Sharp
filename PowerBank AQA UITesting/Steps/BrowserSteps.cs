using BoDi;
using Microsoft.Extensions.Logging;
using NUnit.Allure.Attributes;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.Factory.Browser;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;

namespace PowerBank_AQA_UITesting.Steps
{
    public class BrowserSteps
    {
        private readonly Settings _settings;
        private IObjectContainer _container;
        private readonly IEnumerable<Node> _pages;

        public BrowserSteps(Settings settings, IEnumerable<Node> pages)
        {
            _settings = settings;
            _pages = pages;
            _container = new ObjectContainer();
        }

        [AllureStep("Запущен браузер ")]
        public void StartBrowser()
        {
            var browser = Create(_settings, _pages);
            _container.RegisterInstanceAs(browser);
        }

        [AllureStep("Закрыта веб-страница ")]
        public void CloseWebPage()
        {
            _container.Resolve<IBrowser>().Close();
        }

        [AllureStep("Закрыть браузер ")]
        public void CloseBrowser()
        {
            _container.Resolve<IBrowser>().Quit();
        }

        [AllureStep("Переход драйвера на страницу ")]
        public void SetCurrentPage(string name)
        {
            _container.Resolve<IBrowser>().SetCurrentPage(name);
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

        [AllureStep("Получен браузер ")]
        public IBrowser GetBrowser()
        {
            return _container.Resolve<IBrowser>();
        }

        [AllureStep("Переключение на вкладку ")]
        public void SwitchTo(int number)
        {
            Log.Logger().LogInformation($"Переключение на вкладку {number}");
            _container.Resolve<IBrowser>().SwitchTo(number);
        }

        [AllureStep("Обновить страницу на:")]
        public void UpdateCurrentPage(string name)
        {
            _container.Resolve<IBrowser>().SetCurrentPage(name, false);
        }

        [AllureStep("Установлен размер окна ширина: {0} высота: {1}")]
        public void WindowSize(int width, int height)
        {
            Log.Logger().LogInformation($"Установлен размер окна браузера {width}, {height}");
            _container.Resolve<IBrowser>().WindowSize(width, height);
        }

        [AllureStep("Переход назад ")]
        public void Back()
        {
            _container.Resolve<IBrowser>().Back();
        }

        [AllureStep("Закрыт браузер ")]
        public void Close()
        {
            _container.Resolve<IBrowser>().Close();
        }

        [AllureStep("Переход вперед ")]
        public void Forward()
        {
            Log.Logger().LogInformation("Осуществлен переход на следующую страницу");
            _container.Resolve<IBrowser>().Forward();
        }

        [AllureStep("Установлен максимальный размер окна ")]
        public void Maximize()
        {
            Log.Logger().LogInformation("Окно браузера установлено как максимальное");
            _container.Resolve<IBrowser>().Maximize();
        }

        [AllureStep("Обновить страницу ")]
        public void Refresh()
        {
            Log.Logger().LogInformation("Обнавление страницы");
            _container.Resolve<IBrowser>().Refresh();
        }

        [AllureStep("Переход на страницу ")]
        public void GoToPage(string url)
        {
            Log.Logger().LogInformation($"Переход по URL {url}");
            _container.Resolve<IBrowser>().GoToPage(url);
        }

        [AllureStep("Получить Url текущей страницы")]
        public string GetCurrentUrl()
        {
            Log.Logger().LogInformation($"Получен Url текущей страницы:  {_container.Resolve<IBrowser>().Url}");
            return _container.Resolve<IBrowser>().Url;
        }
    }
}
