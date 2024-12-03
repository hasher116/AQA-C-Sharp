using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_UITestingCore.Models.Mediator
{
    public class ElementMediator : Mediator
    {
        public ElementMediator(int timeout, IWebDriver driver) : base(timeout, driver, new List<Type>()
        {
            typeof(StaleElementReferenceException),
            typeof(ElementClickInterceptedException),
            typeof(ElementNotInteractableException),
            typeof(InvalidElementStateException),
            typeof(NoSuchElementException)

        })
        {
        }
    }
}
