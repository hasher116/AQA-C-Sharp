using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Infrastructures;
using System.Collections.ObjectModel;

namespace PowerBank_AQA_UITestingCore.Models.Providers.Interfaces
{
    public interface IDriverProvider
    {
        Settings.Settings Settings { get; set; }

        string PageSource { get; }

        string Title { get; }

        string Url { get; }

        int Tabs { get; }

        string CurrentWindowHandle { get; }

        ReadOnlyCollection<string> WindowHandles { get; }

        void CreateDriver(Func<IWebDriver> action, Settings.Settings settings);

        IWebDriver GetDriver();

        void Close();

        void Quit();

        IElementProvider GetElement(string locator, How how);

        IEnumerable<IElementProvider> GetElements(string locator, How how);

        void WindowSize(int width, int height);

        void Maximize();

        void Back();

        void Forward();

        void GoToUrl(string url);

        void Refresh();

        void SwitchTo(int number);

        IAlertProvider GetAlert();

        IDriverProvider GetDefaultFrame();

        IDriverProvider GetParentFrame();

        IDriverProvider GetFrame(int id);

        IDriverProvider GetFrame(string name);

        IDriverProvider GetFrame(By by);

        byte[] Screenshot();
    }
}
