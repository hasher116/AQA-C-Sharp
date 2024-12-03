using BoDi;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Helpers;
using PowerBank_AQA_UITestingCore.Infrastructures;
using PowerBank_AQA_UITestingCore.Models.PageObject;

namespace PowerBank_AQA_UITesting.Tests
{
    public class BaseTest
    {
        protected IObjectContainer settingsContainer;
        protected IObjectContainer pageContainer;

        [OneTimeSetUp]
        public void InitializeConfiguration()
        {
            var settings = ConfigOptionsFactory.Create(Configuration.GetConfiguration());
            settingsContainer = new ObjectContainer();
            pageContainer = new ObjectContainer();

            if (settings.Value is null)
            {
                Log.Logger().LogInformation($@"appsettings не содержит {Constants.CONFIG_BLOCK} блок. Выбраны стандартные настройки");
            }
            else
            {
                Log.Logger().LogInformation($@"appsettings содержит {Constants.CONFIG_BLOCK} блок. Выбраны настройки из файла");
            }

            settingsContainer.RegisterInstanceAs(settings.Value);
        }

        [OneTimeSetUp]
        public void InitializePageObject()
        {
            var pageObject = new PageObject();

            pageContainer.RegisterInstanceAs(pageObject.Pages);
        }
    }
}
