using OpenQA.Selenium;

namespace PowerBank_AQA_UITestingCore.Models.Mediator
{
    public class AlertMediator : Mediator
    {
        public AlertMediator(int timeout, IWebDriver driver ) : base(timeout, driver, new List<Type>()
        {
            typeof(NoAlertPresentException)
        })
        {
        }
    }
}
