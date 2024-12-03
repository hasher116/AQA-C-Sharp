using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Exeptions;
using PowerBank_AQA_UITestingCore.Extensions;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.PageObject.Alerts;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;
using PowerBank_AQA_UITestingCore.Models.Providers;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;

namespace PowerBank_AQA_UITestingCore.Models.Factory.Browser
{
    public abstract class Browser : IBrowser
    {
        public Settings.Settings Settings { get; init; }

        public IEnumerable<Node> Pages { get; init; }

        public IDriverProvider DriverProvider { get; set; } = new DriverProvider();

        public Node CurrentPage { get; set; }

        public string Url => DriverProvider?.Url;

        public string Title => DriverProvider?.Title;

        public abstract SessionId SessionId { get; protected set; }

        public int? Tabs => DriverProvider?.Tabs;

        public PageObject.Alerts.IAlert Alert()
        {
            return new Alert(DriverProvider, Settings);
        }

        public void Back()
        {
            Log.Logger().LogInformation("Осуществлен переход на предыдущую страницу");
            DriverProvider?.Back();
        }

        public void Close()
        {
            DriverProvider?.Close();
        }

        public void Forward()
        {
            Log.Logger().LogInformation("Осуществлен переход на следующую страницу");
            DriverProvider?.Forward();
        }

        public IPage GetCurrentPage()
        {
            if(CurrentPage == null)
            {
                throw new PageException($"Текущая страница не существует");
            }

            return CurrentPage?.Object as IPage;
        }

        public void GoToPage(string url)
        {
            Log.Logger().LogInformation($"Переход по URL {url}");
            DriverProvider?.GoToUrl(url);
        }

        public void Maximize()
        {
            Log.Logger().LogInformation("Окно браузера установлено как максимальное");
            DriverProvider?.Maximize();
        }

        public void Quit()
        {
            DriverProvider?.Quit();
            DriverProvider = null;
        }

        public void Refresh()
        {
            Log.Logger().LogInformation("Обнавление страницы");
            DriverProvider?.Refresh();
        }

        public void SetCurrentPage(string name, bool loaded = true)
        {
            Log.Logger().LogInformation($"Текущая страница {name} {(loaded ? "с загрузкой элементов" 
                : "без загрузки элементов")}");
            var page = Pages.SearchPageBy(name);
            CurrentPage = page;

            var objectPage = CurrentPage?.Object as Page;
            ArgumentNullException.ThrowIfNull(objectPage);

            objectPage.SetProvider(DriverProvider, Settings);
            objectPage.Root = page;
            objectPage.Local = null;

            if(!loaded)
            {
                return;
            }
            try
            {
                ((Page)CurrentPage.Object).GoToPage();
            }
            catch (Exception)
            {
                throw new PageException($"Переход на страницу {name} в {(CurrentPage.Object as Page)?.Url} не осуществлен");
            }
        }

        public void SwitchTo(int number)
        {
            Log.Logger().LogInformation($"Переключение на вкладку {number}");
            DriverProvider?.SwitchTo(number);
        }

        public void UpdateCurrentPage(string name)
        {
            SetCurrentPage(name, false);
        }

        public void WindowSize(int width, int height)
        {
            Log.Logger().LogInformation($"Установлен размер окна браузера {width}, {height}");
            DriverProvider?.WindowSize(width, height);
        }
    }
}
