using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITesting.Dto;


namespace PowerBank_AQA_UITesting.Tests.Transfers
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US7_1 Переводы по номеру карты")]
    [AllureFeature("US7_1 Переводы по номеру карты")]
    [AllureEpic("US7_1 Переводы по номеру карты")]
    public class US7_1_TransfersByCardNumber : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;
        private readonly string loginPhone = "6666666666";
        private readonly string password = "Ihave6Cards!";
        private readonly string senderCard = "5123451104453485";
        private readonly string recipientCard = "5123459569776203";
        private readonly string recipientCardSameCardNumber = "2123451947067742";
        private readonly string cvv = "123";
        private readonly string amount = "10";
        private readonly string excessAmount = "40000";
        private readonly string comment = "На мороженое";

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
        }

        [AllureStory("Перевод средств по номеру карты авторизованным пользователем")]
        [AllureStep("Перевод средств по номеру карты авторизованным пользователем")]
        [Test, Order(1)]
        public void TransferByAuthorizedUser()
        {
            TransferInformation expectedTransferInformation = commonSteps.GetInformationBeforeTransfer("Дебетовая карта 3485", recipientCard, amount);
            TransferInformation actualTransferInformation = commonSteps.GetInformationAfterTransfer("Переход на карту 3485", recipientCard, amount, cvv, comment);
            actualTransferInformation.Should().BeEquivalentTo(expectedTransferInformation);
            elementSteps.GetText("Статус перевода").Should().BeEquivalentTo("Успешно выполнен");
            elementSteps.ClickToWebElement("Завершение перевода");
        }

        [AllureStory("Перевод средств по номеру карты с тем же счётом")]
        [AllureStep("Перевод средств по номеру карты с тем же счётом")]
        [Test, Order(2)]
        public void TransferByCardWithSameAccountNumber()
        {
            commonSteps.EnterCardNumberTransfer("Переход на карту 3485", recipientCardSameCardNumber);
            elementSteps.ClickToWebElement("Продолжить");
            elementSteps.IsWebElementDisabled("Продолжить").Should().BeTrue();
            elementSteps.GetText("Ошибка ввода карты").Should().BeEquivalentTo("Карты привязаны к одному счету");
        }

        [AllureStory("Перевод средств по тому же номеру карты, что и у отправителя")]
        [AllureStep("Перевод средств по тому же номеру карты, что и у отправителя")]
        [Test, Order(3)]
        public void TransferByCardWithSameCardNumber()
        {
            commonSteps.EnterCardNumberTransfer("Переход на карту 3485", senderCard);
            elementSteps.IsWebElementDisabled("Продолжить").Should().BeTrue();
            elementSteps.GetText("Ошибка ввода карты").Should().BeEquivalentTo("Номера карты отправителя и получателя совпадают");
        }

        [AllureStory("Отправка отчёта о переводе на email")]
        [AllureStep("Отправка отчёта о переводе на email")]
        [Test, Order(4)]
        public void TransferByCardWithEmailReport()
        {
            commonSteps.EnterCardNumberTransfer("Переход на карту 3485", recipientCard);
            elementSteps.ClickToWebElement("Продолжить");
            commonSteps.EnterAmountAndCvvTransfer(amount, cvv);
            elementSteps.ClickToWebElement("Перевести");
            elementSteps.ClickToWebElement("Чекбокс отправка на почту");
            elementSteps.ClickToWebElement("Завершение перевода");
            elementSteps.GetText("Результат отправки отчёта").Should().BeEquivalentTo("Успешно отправлено");
        }

        [AllureStory("Перевод средств по номеру карты с превышением баланса")]
        [AllureStep("Перевод средств по номеру карты с превышением баланса")]
        [Test, Order(5)]
        public void TransferByCardWithExcessBalance()
        {
            commonSteps.EnterCardNumberTransfer("Переход на карту 3485", recipientCard);
            elementSteps.ClickToWebElement("Продолжить");
            commonSteps.EnterAmountAndCvvTransfer(excessAmount, cvv);
            elementSteps.GetText("Недостаточно средств").Should().Contain("Недостаточно средств");
            elementSteps.IsWebElementDisabled("Перевести").Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }

}