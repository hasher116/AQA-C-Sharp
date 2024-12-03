using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;


namespace PowerBank_AQA_UITesting.Tests.RegistrationAutomationSecurity
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US1_7 Банкоматы и отделения")]
    [AllureFeature("US1_7 Банкоматы и отделения")]
    [AllureEpic("US1_7 Банкоматы и отделения")]
    public class US1_7_ATMsAndBranches : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
            elementSteps.ClickToWebElement("Банкоматы и отделения");
            browserSteps.SetCurrentPage("DepartmentsPage");
        }

        [AllureStory("Отображение и работа кнопки \"+\"")]
        [AllureStep("Отображение и работа кнопки \"+\"")]
        [Test, Order(1)]
        public void CheckUpscalingByButton()
        {
            elementSteps.IsWebElementDisplayed("Увеличение масштаба").Should().BeTrue();
            elementSteps.ClickToWebElement("Куки закрыть");
            elementSteps.ClickToWebElement("Увеличение масштаба");
            elementSteps.GetScale("Масштаб карты").Should().Contain("scale(8192)");
        }

        [AllureStory("Отображение и работа увеличения масштаба")]
        [AllureStep("Отображение и работа увеличения масштаба")]
        [Test, Order(2)]
        public void CheckUpscalingByMouse()
        {
            elementSteps.IsWebElementDisplayed("Увеличение масштаба").Should().BeTrue();
            elementSteps.ClickToWebElement("Куки закрыть");
            elementSteps.SendScrollMouse("Карта", -100);
            elementSteps.GetScale("Масштаб карты").Should().Contain("scale(8192)");
        }

        [AllureStory("Появление сообщения cookies и варианты работы с ними")]
        [AllureStep("Появление сообщения cookies и варианты работы с ними")]
        [Test, Order(3)]
        public void CheckCookieProcessing()
        {
            elementSteps.IsWebElementDisplayed("Куки окно").Should().BeTrue();
            elementSteps.ClickToWebElement("Куки условия");
            browserSteps.SwitchTo(1);
            browserSteps.GetCurrentUrl().Should().Be(configuration["baseUrl"] + "coming-soon");
            browserSteps.Close();
            browserSteps.SwitchTo(0);
            elementSteps.ClickToWebElement("Куки данные");
            browserSteps.SwitchTo(1);
            browserSteps.GetCurrentUrl().Should().Be(configuration["baseUrl"] + "coming-soon");
        }

        [TearDown]
        public void TearDown()
        {
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }

}