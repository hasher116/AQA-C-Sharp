using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using PowerBank_AQA_UITestingCore.Models.Providers;

namespace PowerBank_AQA_UITestingCore.Models.PageObject.Elements
{
    public abstract class DefaultClick : Element
    {
        public DefaultClick(string name, string locator, bool optional = false) : base(name, locator, optional)
        {

        }

        public virtual void Click()
        {
            if(Enabled && Displayed)
            {
                Mediator.Execute(() => ElementProvider.Click());
            }
            else
            {
                throw new ArgumentException($"Проверьте что элемент {Name} Enabled и Displayed");
            }
        }

        public virtual void DoubleClick()
        {
            if (Enabled && Displayed)
            {
                var action = new Actions(Driver.GetDriver());
                Mediator.Execute(() => action.DoubleClick(((ElementProvider)ElementProvider).WebElement).Build().Perform());
            }
            else
            {
                throw new ArgumentException($"Проверьте что элемент {Name} Enabled и Displayed");
            }
        }

        public virtual void MouseScroll(By locator, int deltaY)
        {
            if (Displayed)
            {
                var action = new Actions(Driver.GetDriver());
                var element = Driver.GetDriver().FindElement(locator);
                var scrollOrigin = new WheelInputDevice.ScrollOrigin
                {
                    Element = element
                };
                Mediator.Execute(() => action.ScrollFromOrigin(scrollOrigin, 0, deltaY).Build().Perform());
            }
            else
            {
                throw new ArgumentException($"Проверьте что элемент {Name} Displayed");
            }
        }
    }
}
