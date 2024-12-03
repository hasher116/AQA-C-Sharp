using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using OpenQA.Selenium.DevTools;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Hooks;
using PowerBank_AQA_UITesting.Pages;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;
using System.Data;
using Microsoft.Extensions.Logging;
using PowerBank_AQA_TestingCore.Helpers;
using PowerBank_AQA_UITesting.Dto;
using Bogus.DataSets;
using PowerBank_AQA_UITestingCore.Models.Factory.Browser;

namespace PowerBank_AQA_UITesting.Tests.Cards
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US4_4 Посмотреть банковские карточные продукты")]
    [AllureFeature("US4_4 Посмотреть банковские карточные продукты")]
    [AllureEpic("US4_4 Посмотреть банковские карточные продукты")]
    public class US4_4_CheckingBankCards : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;
        private DbClient dbClientCards;
        private readonly string loginPhone = "9772345685";
        private readonly string password = "ls23Ghq#wEr";
        private readonly string dbColumnCardsName = "Name";

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            dbClientCards = new DbClient(configuration, "CardDb");
            dbClientCards.Create();
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
            commonSteps.Authorization(loginPhone, password);
            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Карты");
            browserSteps.UpdateCurrentPage("CardsPage");

            CardShortInformation cardShortInformation = new CardShortInformation();
        }

        [AllureStory("Общий просмотр банковских карточных продуктов")]
        [AllureStep("Общий просмотр банковских карточных продуктов")]
        [Test]
        public void CheckDifferentBankCardProducts()
        {
            elementSteps.IsWebElementDisplayed("CardProducts").Should().BeTrue();

            var listDebitCards = DbQueries.GetCardsInformation(dbClientCards, "debet");
            commonSteps.CheckCardExistsOnPage(listDebitCards, "ButtonCardSolid", "LabelCardSolid", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listDebitCards, "ButtonCardGame", "LabelCardGame", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listDebitCards, "ButtonCardPowerDrive", "LabelCardPowerDrive", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listDebitCards, "ButtonCardAirlines", "LabelCardAirlines", dbColumnCardsName);

            elementSteps.ClickToWebElement("ButtonCredit");

            var listCreditCards = DbQueries.GetCardsInformation(dbClientCards, "credit");
            commonSteps.CheckCardExistsOnPage(listCreditCards, "ButtonCardShopping", "LabelCardShopping", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listCreditCards, "ButtonCardCache", "LabelCardCache", dbColumnCardsName);

            elementSteps.ClickToWebElement("ButtonVirtual");

            var listVirtualCards = DbQueries.GetCardsInformation(dbClientCards, "virtual");
            commonSteps.CheckCardExistsOnPage(listVirtualCards, "ButtonCardSuper", "LabelCardSuper", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listVirtualCards, "ButtonCardPower", "LabelCardPower", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listVirtualCards, "ButtonCardLady", "LabelCardLady", dbColumnCardsName);
            commonSteps.CheckCardExistsOnPage(listVirtualCards, "ButtonCardNeon", "LabelCardNeon", dbColumnCardsName);
        }

        [AllureStory("Просмотр краткой информации по дебетовым продуктам")]
        [AllureStep("Просмотр краткой информации по дебетовым продуктам")]
        [Test]
        public void CheckShortInfoAboutDebitProducts()
        {
            elementSteps.ClickToWebElement("ButtonDebit");

            string cardsName = elementSteps.GetText("CardLabel");
            string cardsType = elementSteps.GetText("CardType");
            string cardsShortDescription = elementSteps.GetText("CardShortDescription");
            string cardsCashBack = TrimString.GetNumbersInString(elementSteps.GetText("CardCashback"));
            string cardsServicePrice = TrimString.GetNumbersInString(elementSteps.GetText("CardServicePrice"));

            CardShortInformation card = new CardShortInformation();
            card.CardName = cardsName;
            card.CardType = cardsType;
            card.CardShortDescription = cardsShortDescription;
            card.CardServicePrice = cardsServicePrice;
            card.CardCashBack = cardsCashBack;

            var debitCard = DbQueries.GetCardInformation(dbClientCards, card.CardName);
            commonSteps.CheckShortInfoAboutCardDebitAndVirtual(debitCard, card);
        }

        [AllureStory("Просмотр краткой информации по кредитным продуктам")]
        [AllureStep("Просмотр краткой информации по кредитным продуктам")]
        [Test]
        public void CheckShortInfoAboutCreditProducts()
        {
            elementSteps.ClickToWebElement("ButtonCredit");

            string cardsName = elementSteps.GetText("CardLabel");
            string cardsType = elementSteps.GetText("CardType");
            string cardsShortDescription = elementSteps.GetText("CardShortDescription");
            string cardsInterestRate = TrimString.GetNumbersInString(elementSteps.GetText("CardInterestRate"));
            string cardsInterestFreeDay = TrimString.GetNumbersInString(elementSteps.GetText("CardInterestFreeDay"));
            string cardsServicePrice = TrimString.GetNumbersInString(elementSteps.GetText("CardServicePrice"));

            CardShortInformation card = new CardShortInformation();
            card.CardName = cardsName;
            card.CardType = cardsType;
            card.CardShortDescription = cardsShortDescription;
            card.CardInterestRate = cardsInterestRate;
            card.CardInterestFreeDays = cardsInterestFreeDay;
            card.CardServicePrice = cardsServicePrice;

            var creditCard = DbQueries.GetCardInformation(dbClientCards, card.CardName);
            commonSteps.CheckShortInfoAboutCardCredit(creditCard, card);
        }

        [AllureStory("Просмотр краткой информации по виртуальным продуктам")]
        [AllureStep("Просмотр краткой информации по виртуальным продуктам")]
        [Test]
        public void CheckShortInfoAboutVirtualProducts()
        {
            elementSteps.ClickToWebElement("ButtonVirtual");

            string cardsName = elementSteps.GetText("CardLabel");
            string cardsType = elementSteps.GetText("CardType");
            string cardsShortDescription = elementSteps.GetText("CardShortDescription");
            string cardsCashBack = TrimString.GetNumbersInString(elementSteps.GetText("CardCashback"));
            string cardsServicePrice = TrimString.GetNumbersInString(elementSteps.GetText("CardServicePrice"));

            CardShortInformation card = new CardShortInformation();
            card.CardName = cardsName;
            card.CardType = cardsType;
            card.CardShortDescription = cardsShortDescription;
            card.CardServicePrice = cardsServicePrice;
            card.CardCashBack = cardsCashBack;

            var virtualCard = DbQueries.GetCardInformation(dbClientCards, card.CardName);
            commonSteps.CheckShortInfoAboutCardDebitAndVirtual(virtualCard, card);
        }

        [AllureStory("Просмотр подробной информации по дебетовому продукту")]
        [AllureStep("Просмотр подробной информации по дебетовому продукту")]
        [Test]
        public void CheckDetaliedInfoAboutDebitProducts()
        {
            elementSteps.ClickToWebElement("ButtonDebit");
            elementSteps.ClickToWebElement("ButtonCardSolid");
            browserSteps.UpdateCurrentPage("DetailedCardPage");

            CardDetailedInformation actualCardDetailedInformation = new CardDetailedInformation()
            {
                CardName = elementSteps.GetText("CardLabel"),
                CardType = elementSteps.GetText("CardType"),
                CardCashBack = TrimString.GetNumbersInString(elementSteps.GetText("CardCashBack")).ToString(),
                CardServicePrice = TrimString.GetNumbersInString(elementSteps.GetText("CardServicePrice")).ToString(),
                CardCashMaxDay = TrimString.GetNumbersInString(elementSteps.GetText("CardCashMaxDay")).ToString(),
                CardCashMaxMonth = TrimString.GetNumbersInString(elementSteps.GetText("CardCashMaxMonth")).ToString(),
                CardNotificationPrice = TrimString.GetNumbersInString(elementSteps.GetText("CardNotificationPrice")).ToString(),
                CardPaymentSystem = elementSteps.GetText("CardPaymentSystem").ToLowerInvariant()
            };

            var cardSolid = DbQueries.GetCardInformation(dbClientCards, actualCardDetailedInformation.CardName);
            commonSteps.CheckDetailedInfoAboutCardDebit(cardSolid, actualCardDetailedInformation);
        }

        [AllureStory("Просмотр подробной информации по кредитному продукту")]
        [AllureStep("Просмотр подробной информации по кредитному продукту")]
        [Test]
        public void CheckDetaliedInfoAboutCreditProducts()
        {
            elementSteps.ClickToWebElement("ButtonCredit");
            elementSteps.ClickToWebElement("ButtonCardShopping");
            browserSteps.UpdateCurrentPage("DetailedCardPage");

            CardDetailedInformation actualCardDetailedInformation = new CardDetailedInformation()
            {
                CardName = elementSteps.GetText("CardLabel"),
                CardType = elementSteps.GetText("CardType"),
                CardInterestRate = TrimString.GetNumbersInString(elementSteps.GetText("CardInterestRate")).ToString(),
                CardServicePrice = TrimString.GetNumbersInString(elementSteps.GetText("CardServicePrice")).ToString(),
                CardInterestFreeDays = TrimString.GetNumbersInString(elementSteps.GetText("CardInterestFreeDays")).ToString(),
                CardAmountCreditMax = TrimString.GetNumbersInString(elementSteps.GetText("CardAmountCreditMax")).ToString(),
                CardNotificationPrice = TrimString.GetNumbersInString(elementSteps.GetText("CardNotificationPrice")).ToString(),
                CardPaymentSystem = elementSteps.GetText("CardPaymentSystem").ToLowerInvariant(),
                CardCashWithdrawalFee = TrimString.GetNumbersInString(elementSteps.GetText("CardCashWithdrawalFee")).ToString(),
                CardCashMaxDay = TrimString.GetNumbersInString(elementSteps.GetText("CardCashMaxDay")).ToString(),
            };

            var cardShopping = DbQueries.GetCardInformation(dbClientCards, actualCardDetailedInformation.CardName);
            commonSteps.CheckDetailedInfoAboutCardCredit(cardShopping, actualCardDetailedInformation);
        }

        [AllureStory("Просмотр подробной информации по виртуальному продукту")]
        [AllureStep("Просмотр подробной информации по виртуальному продукту")]
        [Test]
        public void CheckDetaliedInfoAboutVirtualProducts()
        {
            elementSteps.ClickToWebElement("ButtonVirtual");
            elementSteps.ClickToWebElement("ButtonCardLady");
            browserSteps.UpdateCurrentPage("DetailedCardPage");

            CardDetailedInformation actualCardDetailedInformation = new CardDetailedInformation()
            {
                CardName = elementSteps.GetText("CardLabel"),
                CardType = elementSteps.GetText("CardType"),
                CardCashBack = TrimString.GetNumbersInString(elementSteps.GetText("CardCashBack")),
                CardServicePrice = TrimString.GetNumbersInString(elementSteps.GetText("CardServicePrice")),
                CardNotificationPrice = TrimString.GetNumbersInString(elementSteps.GetText("CardNotificationPrice")),
                CardPaymentSystem = elementSteps.GetText("CardPaymentSystem").ToLowerInvariant()
            };

            var cardLady = DbQueries.GetCardInformation(dbClientCards, actualCardDetailedInformation.CardName);
            commonSteps.CheckDetailedInfoAboutCardVirtual(cardLady, actualCardDetailedInformation);
        }

        [AllureStory("Просмотр тарифов по карте PDF")]
        [AllureStep("Просмотр тарифов по карте PDF")]
        [Test]
        public void CheckCardTariffs()
        {
            elementSteps.ClickToWebElement("ButtonDebit");
            elementSteps.ClickToWebElement("ButtonCardGame");
            browserSteps.UpdateCurrentPage("DetailedCardPage");
            elementSteps.ClickToWebElement("CardTariff");
            browserSteps.SwitchTo(1);
            browserSteps.GetCurrentUrl().Should().Contain("CardTarifs").And.Contain(".pdf");
        }

        [AllureStory("Просмотр условий выпуска и обслуживания по карте PDF")]
        [AllureStep("Просмотр условий выпуска и обслуживания по карте PDF")]
        [Test]
        public void CheckCardUsingConditions()
        {
            elementSteps.ClickToWebElement("ButtonCredit");
            elementSteps.ClickToWebElement("ButtonCardCache");
            browserSteps.UpdateCurrentPage("DetailedCardPage");
            elementSteps.ClickToWebElement("CardService");
            browserSteps.SwitchTo(1);
            browserSteps.GetCurrentUrl().Should().Contain("CardUsingConditions.pdf");
        }

        [AllureStory("Возврат к перечню продуктов")]
        [AllureStep("Возврат к перечню продуктов")]
        [Test]
        public void ReturnToVirtualCardProducts()
        {
            elementSteps.ClickToWebElement("ButtonVirtual");
            elementSteps.ClickToWebElement("ButtonCardSuper");
            browserSteps.Back();
            elementSteps.IsEnabledButton("ButtonVirtual").Should().BeTrue();
        }

        [Test, Ignore("Тест с коллекциями. Hold по починки получения коллекций")]
        public void CheckShortInfoAboutDebitProductsNew()
        {
            elementSteps.ClickToWebElement("ButtonDebit");

            var debitCards = DbQueries.GetCardsInformation(dbClientCards, "debet");
            commonSteps.CompareShortCardInfo(debitCards);
        }

        [TearDown]
        public void TearDown()
        {
            dbClientCards.Dispose();
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }
}
