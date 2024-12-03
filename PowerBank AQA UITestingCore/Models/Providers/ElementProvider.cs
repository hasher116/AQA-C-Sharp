using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using PowerBank_AQA_UITestingCore.Exeptions;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text.RegularExpressions;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Logging;

namespace PowerBank_AQA_UITestingCore.Models.Providers
{
    public class ElementProvider : IElementProvider
    {
        private int _timeout;
        private By _locator;

        private IWebElement _element;

        public ElementProvider(int timeout, By locator)
        {
            _timeout = timeout;
            _locator = locator;
        }

        public IWebElement WebElement
        {
            get
            {
                if (_element is not null)
                {
                    return _element;
                }

                try
                {
                    _element = WebDriver.FindElement(_locator);
                }
                catch (NoSuchElementException ex)
                {
                    Log.Logger().LogDebug(ex.Message);
                    return null;
                }

                return _element;
            }
            set => _element = value;
        }

        public IWebDriver WebDriver { get; init; }

        public bool Displayed => WebElement.Displayed;

        public bool NotDisplayed => !WebElement.Displayed;

        public bool Selected => WebElement.Selected;

        public bool NotSelected => !WebElement.Selected;

        public bool Enabled => WebElement.Enabled;

        public bool Disabled => !WebElement.Enabled;

        public bool Loaded => WebElement is not null;

        public bool NotLoaded => WebElement is null;

        public bool Editable => IsEditable();

        public Point Location => WebElement.Location;

        public string Text => WebElement.Text;

        public string Tag => WebElement.TagName;

        public void Clear()
        {
            try
            {
                WebElement.Clear();
            }
            catch (Exception ex)
            {
                throw new ElementException($"Clear element is return error with message {ex.Message}");
            }
        }

        public void Click()
        {
            try
            {
                WebElement.Click();
            }
            catch (Exception ex)
            {
                throw new ElementException($"Click element is return error with message {ex.Message}");
            }
        }

        public bool TextEqual(string text)
        {
            try
            {
                return new WebDriverWait(WebDriver, TimeSpan.FromSeconds(_timeout))
                .Until(_ => string.Equals(WebElement.Text, text));
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{WebElement.Text}\" is not equal \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextContain(string text)
        {
            try
            {
                return new WebDriverWait(WebDriver, TimeSpan.FromSeconds(_timeout))
                .Until(_ => WebElement.Text.Contains(text));
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{WebElement.Text}\" is not contain \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public bool TextMatch(string text)
        {
            try
            {
                var regex = new Regex(text);
                return new WebDriverWait(WebDriver, TimeSpan.FromSeconds(_timeout))
                .Until(_ => regex.Match(WebElement.Text).Success);
            }
            catch (WebDriverTimeoutException ex)
            {
                Log.Logger().LogWarning($"\"{WebElement.Text}\" is not match \"{text}\". Exception is {ex.Message}");
                return false;
            }
        }

        public IElementProvider FindElement(By by)
        {
            var element = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(_timeout))
            .Until(_ => WebElement.FindElement(by));

            return new ElementProvider(_timeout, by)
            {
                WebElement = element
            };
        }

        public ReadOnlyCollection<IElementProvider> FindElements(By by)
        {
            var elements = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(_timeout))
            .Until(_ => WebElement.FindElements(by));
            var listElement = elements.Select(element => new ElementProvider(_timeout, by) { WebElement = element }).Cast<IElementProvider>().ToList();
            return listElement.AsReadOnly();
        }

        public string GetAttribute(string name)
        {
            try
            {
                Log.Logger().LogDebug($"Get attribute by name \"{name}\"");
                return WebElement.GetAttribute(name);
            }
            catch (Exception ex)
            {
                throw new ElementException($"Get attribute by name \"{name}\" is return error with message {ex.Message}");
            }
        }

        public string GetCss(string name)
        {
            try
            {
                return WebElement.GetCssValue(name);
            }
            catch (Exception ex)
            {
                throw new ElementException($"GetCssValue by name \"{name}\" is return error with message {ex.Message}");
            }
        }

        public void SendKeys(string keys)
        {
            try
            {
                WebElement.SendKeys(keys);
            }
            catch (Exception ex)
            {
                throw new ElementException($"SendKeys \"{keys}\" in element is return error with message {ex.Message}");
            }
        }

        public void WaitUntilAttributeValueEquals(string attributeName, string attributeValue)
        {
            WebElement = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(_timeout))
            .Until(
            _ => WebElement.GetAttribute(attributeName) == attributeValue
            ? WebElement
            : throw new ElementException($"Waiting until attribute \"{attributeName}\" becomes value \"{attributeValue ?? "null"}\" is failed"));
        }

        private bool IsEditable()
        {
            return !Convert.ToBoolean(GetAttribute("readonly"));
        }
    }
}
