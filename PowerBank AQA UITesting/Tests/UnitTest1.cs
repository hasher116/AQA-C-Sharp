using FluentAssertions;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.Mediator;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.PageObject.Alerts;
using PowerBank_AQA_UITestingCore.Models.Providers.Interfaces;
using PowerBank_AQA_UITestingCore.Models.Settings;

namespace PowerBank_AQA_UITesting.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("OnlinerTest")]
    [AllureFeature("OnlinerTest")]
    [AllureEpic("OnlinerTest")]
    public class Tests : BaseTest
    {
        private BrowserSteps bSteps;
        private ElementSteps eSteps;

        [SetUp]
        public void Setup()
        {
            bSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
        }

        [AllureStory("OnlinerTest")]
        [AllureStep("OnlinerTest")]
        [Test, Ignore("Foo test")]
        public void OnlinerTest()
        {
            bSteps.StartBrowser();
            bSteps.SetCurrentPage("Main");
            eSteps = new ElementSteps(bSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            bSteps.WindowSize(1100, 700);
            eSteps.IsWebElementDisplayed("Смартфоны").Should().Be(true);
            eSteps.ClickToWebElement("Каталог");
            bSteps.Back();
            //    //bSteps.Forward();
            bSteps.UpdateCurrentPage("Main");
            //    eSteps.ClickToWebElement("Каталог");
            //    bSteps.SwitchTo(0);
            bSteps.Refresh();

        }

        [TearDown]
        public void TearDown()
        {
            bSteps.CloseWebPage();
            bSteps.CloseBrowser();
        }
    }
}