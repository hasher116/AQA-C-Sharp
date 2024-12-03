using OpenQA.Selenium;

namespace PowerBank_AQA_UITestingCore.Models.Mediator
{
    public class FrameMediator : Mediator
    {
        public FrameMediator(int timeout, IWebDriver driver) : base(timeout, driver, new List<Type>()
        {
            typeof(StaleElementReferenceException),
            typeof(ElementClickInterceptedException),
            typeof(ElementNotInteractableException),
            typeof(InvalidElementStateException),
            typeof(NoSuchFrameException)

        })
        {
        }
    }
}
