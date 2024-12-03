using OpenQA.Selenium;

namespace PowerBank_AQA_UITestingCore.Models.Mediator
{
    public class StandartMediator : Mediator
    {
        public StandartMediator(int timeout, IWebDriver driver) : base(timeout, driver)
        {
        }
    }
}
