using OpenQA.Selenium.Support.UI;
using PowerBank_AQA_UITestingCore.Models.Factory.Browser;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.Settings;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;

namespace PowerBank_AQA_SpecFlow.StepDefinitions
{
    public class ElementStepDefinitions
    {
        private readonly IBrowser browser;
        private readonly Settings settings;
        public ElementStepDefinitions(IBrowser browser, Settings settings)
        {
            this.browser = browser;
            this.settings = settings;
        }
        public void ClickToWebElement(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is DefaultClick).Should().BeTrue();
            (element as DefaultClick)?.Click();
        }

        public void SendKeys(string name, string textForSet)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is Input).Should().BeTrue();
            (element as Input).SetText(textForSet);
        }

        public string GetText(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is Element).Should().BeTrue();
            PowerBank_AQA_TestingCore.Helpers.Log.Logger().LogInformation($"Получен текст элемента {name}:  {element.Text}");
            return (element as Element).Text;
        }

        public bool IsWebElementEnabled(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is DefaultClick).Should().BeTrue();
            return ((element as DefaultClick).Enabled);
        }
    }
}
