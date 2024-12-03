using BoDi;
using PowerBank_AQA_SpecFlow.StepDefinitions;
using PowerBank_AQA_UITestingCore.Models.Settings;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using TechTalk.SpecFlow;
using PowerBank_AQA_UITestingCore.Helpers;
using PowerBank_AQA_TestingCore.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace PowerBank_AQA_SpecFlow.Support
{
    [Binding]
    public sealed class Hooks
    {
        private BrowserStepDefinitions browserStepDefinitions;
        private ElementStepDefinitions elementStepDefinitions; 
        private IObjectContainer _settingsContainer;
        private IObjectContainer _pageContainer;
        private IConfiguration configuration;
        private static readonly ConcurrentBag<IObjectContainer> TestThreadContainers = new();
        public Hooks(IObjectContainer settingsContainer, IObjectContainer pageContainer)
        {
            _settingsContainer = settingsContainer;
            _pageContainer = pageContainer;
        }

        [BeforeScenario(Order = -1)]
        public void EnsureTestThreadContainerRegistered(ScenarioContext scenarioContext)
        {
            var testThreadContainer =
                scenarioContext.ScenarioContainer.Resolve<TestThreadContext>().TestThreadContainer;
            TestThreadContainers.Add(testThreadContainer);
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario()
        {
            var settings = ConfigOptionsFactory.Create(Configuration.GetConfiguration());
            _settingsContainer = new ObjectContainer();
            _pageContainer = new ObjectContainer();
            _settingsContainer.RegisterInstanceAs(settings.Value);
            var pageObject = new PageObject();
            _pageContainer.RegisterInstanceAs(pageObject.Pages);
            browserStepDefinitions = new BrowserStepDefinitions(_settingsContainer.Resolve<Settings>(), TestThreadContainers.First(), _pageContainer.Resolve<IEnumerable<Node>>());
            browserStepDefinitions.StartBrowser();
            elementStepDefinitions = new ElementStepDefinitions(browserStepDefinitions.GetBrowser(), _settingsContainer.Resolve<Settings>());
        }

        [AfterScenario]
        public void AfterScenario()
        {
            browserStepDefinitions.CloseBrowser();
        }

        [BeforeScenario]
        public void BeforeScenario(IObjectContainer objectContainer)
        {
            var pageObject = new PageObject();
            objectContainer.RegisterInstanceAs(pageObject.Pages);
        }

        public void InitializeConfiguration(IObjectContainer objectContainer)
        {
            configuration = Configuration.GetConfiguration();
            objectContainer.RegisterInstanceAs(configuration);
        }

        [AfterTestRun]
        public static void DisposeTestThreadContainers()
        {
            var containers = TestThreadContainers.ToArray();
            foreach (var container in containers)
            {
                container.Dispose();
            }
        }
    }
}