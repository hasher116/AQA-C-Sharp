using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PowerBank_AQA_UITestingCore.Models.Mediator
{
    public abstract class Mediator : IMediator
    {
        private readonly int timeout;
        private readonly IWebDriver driver;
        private readonly List<Type> ignoreTypes;

        public Mediator(int timeout, IWebDriver driver, List<Type> ignoreTypes = null)
        {
            this.timeout = timeout;
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.ignoreTypes = ignoreTypes ?? throw new ArgumentNullException(nameof(ignoreTypes));
        }

        public void Execute(Action action, int? timeout = null)
        {
            var t  = timeout ?? this.timeout;

            var driverWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(t),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            if(ignoreTypes is not null)
            {
                driverWait.IgnoreExceptionTypes(ignoreTypes.ToArray());
            }

            driverWait.Until(_ =>
            {
                action.Invoke();
                return true;
            });
        }

        public object Execute<TResult>(Func<TResult> action, int? timeout = null)
        {
            var t = timeout ?? this.timeout;

            var driverWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(t),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };
            if (ignoreTypes is not null)
            {
                driverWait.IgnoreExceptionTypes(ignoreTypes.ToArray());
            }

            var result = driverWait.Until(_ => action.Invoke());
            return result;
        }

        public object Wait<TResult>(Func<TResult> action, int? timeout = null)
        {
            var t = timeout ?? this.timeout;

            var driverWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(t),
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };
            if (ignoreTypes is not null)
            {
                driverWait.IgnoreExceptionTypes(ignoreTypes.ToArray());
            }

            var result = driverWait.Until(_ => action.Invoke());
            return result;
        }
    }
}
