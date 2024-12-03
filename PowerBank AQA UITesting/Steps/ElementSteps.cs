using FluentAssertions;
using NUnit.Allure.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PowerBank_AQA_UITestingCore.Models.Factory.Browser;
using PowerBank_AQA_UITestingCore.Models.PageObject.Elements;
using PowerBank_AQA_UITestingCore.Models.Settings;
using Microsoft.Extensions.Logging;
using Bogus;

namespace PowerBank_AQA_UITesting.Steps
{
    public class ElementSteps
    {
        private readonly IBrowser browser;
        private readonly Settings settings;
        public ElementSteps(IBrowser browser, Settings settings)
        {
            this.browser = browser;
            this.settings = settings;
        }

        [AllureStep("Нажат элемент ")]
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

        [AllureStep("Отображается ли элемент")]
        public bool IsWebElementDisplayed(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is Element).Should().BeTrue();
            return ((element as Element).Displayed);
        }

        [AllureStep("Ввести текст в поле")]
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

        [AllureStep("Получить значение аттрибута")]
        public string GetAttributeValue(string name, string attributeName)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is Element).Should().BeTrue();

            return (element as Element).GetAttribute(attributeName);
        }

        [AllureStep("Получить текст элемента")]
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

        [AllureStep("Принять алерт")]
        public void AcceptAlert()
        {
            var alert = browser.Alert();
            alert.Accept();
        }

        [AllureStep("Отклонить алерт")]
        public void DismissAlert()
        {
            var alert = browser.Alert();
            alert.Dissmiss();
        }

        [AllureStep("Принять алерт")]
        public void SendKeysToAlert(string keys)
        {
            var alert = browser.Alert();
            alert.SendKeys(keys);
        }

        [AllureStep("Получить текст из алерта")]
        public string GetTextFromAlert()
        {
            var alert = browser.Alert();
            return alert.Text;
        }

        [AllureStep("Не отображается элемент")]
        public bool IsWebElementNotDisplayed(string name)
        {
            try
            {
                var element = browser.GetCurrentPage().GetElement(name);

                var defaultWait = new DefaultWait<IElement>(element)
                {
                    Timeout = TimeSpan.FromSeconds(1),
                    PollingInterval = TimeSpan.FromMilliseconds(100)
                };

                defaultWait.Until(_ => _.By);
                (element is Element).Should().BeTrue();
                return ((element as Element).Disabled);
            }
            catch (WebDriverTimeoutException)
            //catch (PowerBank_AQA_UITestingCore.Exeptions.SearchException)
            {
                PowerBank_AQA_TestingCore.Helpers.Log.Logger().LogError($"Элемент не отображается на странице");
                return true;
            }
        }

        [AllureStep("Получить блок")]
        public Block GetBlockDisplayed(string blockName)
        {
            var block = browser.GetCurrentPage().GetBlock(blockName);
            Console.WriteLine("sdaf");
            var defaultWait = new DefaultWait<IElement>(block)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (block is Block).Should().BeTrue();
            return (block as Block).GetBlock(blockName);
        }

        [AllureStep("Получить текст коллекции элементов")]
        public List<string> GetTextFromCollection(string name)
        {
            var collection = browser.GetCurrentPage().GetCollection(name);
            List<string> textList = new List<string>();
            foreach (var element in collection)
            {
                var defaultWait = new DefaultWait<IElement>(element)
                {
                    Timeout = TimeSpan.FromSeconds(settings.Timeout),
                    PollingInterval = TimeSpan.FromMilliseconds(100)
                };

                defaultWait.Until(_ => _.Displayed);
                (element is Element).Should().BeTrue();
                textList.Add((element as Element).Text);
            }
            return textList;
        }

        [AllureStep("Проверить активную кнопку")]
        public bool IsEnabledButton(string name)
        {
            return Convert.ToBoolean(browser.GetCurrentPage().GetElement(name).GetAttribute("aria-selected"));
        }

        [AllureStep("Проверить наличие ссылки pdf-файла")]
        public bool IsExistsLink(string name)
        {
            return (browser.GetCurrentPage().GetElement(name).GetAttribute("href").Contains("pdf"));
        }

        [AllureStep("Масштаб")]
        public string GetScale(string name)
        {
            return (browser.GetCurrentPage().GetElement(name).GetAttribute("style"));
        }

        [AllureStep("Прокрутить мышь")]
        public void SendScrollMouse(string name, int deltaY)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element as DefaultClick).MouseScroll(element.By, deltaY);
        }

        [AllureStep("Выбран ли элемент")]
        public bool IsWebElementSelected(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is DefaultClick).Should().BeTrue();
            return ((element as DefaultClick).Selected);
        }

        [AllureStep("Доступен ли элемент для выбора. Проверка на true")]
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

        [AllureStep("Доступен ли элемент для выбора. Проверка на false")]
        public bool IsWebElementDisabled(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is DefaultClick).Should().BeTrue();
            return ((element as DefaultClick).Disabled);
        }

        [AllureStep("Очистить поле ввода")]
        public void ClearInputField(string name)
        {
            var element = browser.GetCurrentPage().GetElement(name);

            var defaultWait = new DefaultWait<IElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(settings.Timeout),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            defaultWait.Until(_ => _.Displayed);
            (element is Input).Should().BeTrue();
            (element as Input).Clear();
        }
    }
}
