using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using OpenQA.Selenium;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;
using System.Text.RegularExpressions;

namespace PowerBank_AQA_UITesting.Tests.Cards
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US4_7 Заказ выписки со счёта")]
    [AllureFeature("US4_7 Заказ выписки со счёта")]
    [AllureEpic("US4_7 Заказ выписки со счёта")]
    public class US4_7_OrderAccountStatement : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;
        private readonly string loginPhone = "9772345685";
        private readonly string password = "ls23Ghq#wEr";

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
            commonSteps.Authorization(loginPhone, password);
            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Карты");
            browserSteps.UpdateCurrentPage("CardsPage");
            elementSteps.ClickToWebElement("CardUser");
            elementSteps.ClickToWebElement("AccountStatement");
        }

        [AllureStory("Общий просмотр страницы выписки по счёту")]
        [AllureStep("Общий просмотр страницы выписки по счёту")]
        [Test, Order(1)]
        public void CheckGeneralViewStatement()
        {
            elementSteps.IsWebElementDisplayed("OrderAccountStatement").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("Back").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("Enter period").Should().BeTrue();
            commonSteps.CompareDateStatement("1 Month");
            elementSteps.IsWebElementDisplayed("Format PDF").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("Format XLSX").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("Format TXT").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("Open and look").Should().BeTrue();
        }

        [AllureStory("Валидация ручного ввода в поле выбора диапазона дат (валидные значения)")]
        [AllureStep("Валидация ручного ввода в поле выбора диапазона дат (валидные значения)")]
        [TestCase("Enter period", "01.01.2020 - 31.12.2022"), Order(2)]
        [TestCase("Enter period", "29.02.2020 - 01.03.2020")]
        public void CheckValidManualInput(string locator, string data)
        {
            commonSteps.ClearDataFieldAndFill(locator, data);
            elementSteps.IsWebElementEnabled("Submit").Should().BeTrue();

        }

        [AllureStory("Валидация ручного ввода в поле выбора диапазона дат (невалидные значения)")]
        [AllureStep("Валидация ручного ввода в поле выбора диапазона дат (невалидные значения)")]
        [TestCase("Enter period", "01.01.2020 - 02.01.2023"), Order(3)]
        [TestCase("Enter period", "00.01.2020 - 00.01.2023")]
        [TestCase("Enter period", "01.01.2023 - 32.01.2023")]
        [TestCase("Enter period", "32.01.2023 - 01.02.2023")]
        [TestCase("Enter period", "01.00.2023 - 01.01.2023")]
        [TestCase("Enter period", "01.01.2023 - 01.00.2023")]
        [TestCase("Enter period", "01.13.2023 - 01.01.2023")]
        [TestCase("Enter period", "01.01.2023 - 01.13.2023")]
        [TestCase("Enter period", "01.01.0000 - 01.01.2023")]
        [TestCase("Enter period", "01.01.2023 - 01.01.0000")]
        [TestCase("Enter period", "01.01.2023 - 01.01.0001")]
        [TestCase("Enter period", "#$.^&.gdfg - yy.$%.)(*&")]
        [TestCase("Enter period", "29.02.2023 - 01.03.2023")]
        public void CheckInvalidManualInput(string locator, string data)
        {
            commonSteps.ClearDataFieldAndFill(locator, data);
            elementSteps.IsWebElementDisabled("Submit").Should().BeTrue();
        }
        
        [AllureStory("Календарь ввода в поле выбора диапазона дат")]
        [AllureStep("Календарь ввода в поле выбора диапазона дат")]
        [Test, Order(4)]
        public void CheckCalendar()
        {
            commonSteps.ClickToCalendar("Start date", "End date");
            elementSteps.IsWebElementEnabled("Submit").Should().BeTrue();
            commonSteps.ClickToCalendar("End date", "Start date");
            elementSteps.IsWebElementDisabled("Submit").Should().BeTrue();
            commonSteps.ClickToCalendar("Next month", 2, "Start date", "End date");
            elementSteps.IsWebElementDisabled("Submit").Should().BeTrue();
            commonSteps.ClickToCalendar("Previous month", 3, "Start date", "Start date");
            elementSteps.IsWebElementEnabled("Submit").Should().BeTrue();
        }

        [AllureStory("Radiobutton в поле выбора диапазона дат")]
        [AllureStep("Radiobutton в поле выбора диапазона дат")]
        [Test, Order(5)]
        public void CheckRadioButtonInput()
        {
            commonSteps.CheckDateRadioButton("1 Month").Should().BeTrue();
            commonSteps.CheckDateRadioButton("2 Month").Should().BeTrue();
            commonSteps.CheckDateRadioButton("6 Month").Should().BeTrue();
        }

        [AllureStory("Проверка соответствия номера счета в выписке")]
        [AllureStep("Проверка соответствия номера счета в выписке")]
        [Test, Order(6)]
        public void CheckСomplianceAccountNumbers()
        {
            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Карты");
            browserSteps.UpdateCurrentPage("CardsPage");
            elementSteps.ClickToWebElement("CardUser");
            string expectedResult = elementSteps.GetText("Account number on page");
            expectedResult = Regex.Replace(expectedResult, "[А-Яа-я :]", "");
            elementSteps.ClickToWebElement("AccountStatement");
            elementSteps.ClickToWebElement("1 Month");
            elementSteps.ClickToWebElement("Submit");
            string actualResult = elementSteps.GetText("Account number in statement");
            actualResult = Regex.Replace(actualResult, "[А-Яа-я :]", "");
            expectedResult.Equals(actualResult).Should().BeTrue();
        }

        [AllureStory("Проверка соответствия периода в поле ввода и выписке")]
        [AllureStep("Проверка соответствия периода в поле ввода и выписке")]
        [Test, Order(7)]
        public void CheckCompliancePeriods()
        {
            elementSteps.ClickToWebElement("1 Month");
            string expectedResult = elementSteps.GetAttributeValue("Enter period", "value");
            expectedResult = Regex.Replace(expectedResult, "[А-Яа-я :]", "");
            elementSteps.ClickToWebElement("Submit");
            string actualResult = elementSteps.GetText("Period");
            actualResult = Regex.Replace(actualResult, "[А-Яа-я :]", "");
            expectedResult.Equals(actualResult).Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }
}
