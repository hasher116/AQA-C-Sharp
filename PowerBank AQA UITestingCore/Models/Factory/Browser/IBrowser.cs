using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Models.PageObject.Pages;
using IAlert = PowerBank_AQA_UITestingCore.Models.PageObject.Alerts.IAlert;

namespace PowerBank_AQA_UITestingCore.Models.Factory.Browser
{
    public interface IBrowser
    {
        Settings.Settings Settings { get; init; }

        string Url { get; }

        string Title { get; }

        SessionId SessionId { get; }

        int? Tabs { get; }

        void Close();

        void Quit();

        void WindowSize(int width, int height);

        void Maximize();

        void Back();

        void Forward();

        void GoToPage(string url);

        void Refresh();

        void SetCurrentPage(string name, bool loaded = true);

        void UpdateCurrentPage(string name);

        IPage GetCurrentPage();

        void SwitchTo(int number);

        IAlert Alert();

        IBrowser UsingTimeout(int timeout)
        {
            Settings.Timeout = timeout;
            return this;
        }
    }
}
