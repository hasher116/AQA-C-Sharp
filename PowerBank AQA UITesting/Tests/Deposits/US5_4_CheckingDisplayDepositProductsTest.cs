using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Hooks;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;
using System.Data;

namespace PowerBank_AQA_UITesting.Tests.Deposits
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US5_4 Посмотреть доступные для оформления депозиты")]
    [AllureFeature("US5_4 Посмотреть доступные для оформления депозиты")]
    [AllureEpic("US5_4 Посмотреть доступные для оформления депозиты")]
    public class US5_4_CheckingDisplayDepositProductsTest : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;
        private DbController dbController;
        private DbClient dbClientDeposit;
        private readonly string loginPhone = "6666666666";
        private readonly string password = "Ihave6Cards!";
        private readonly string dbColumnDepositsName = "Name";

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            dbClientDeposit = new DbClient(configuration, "DepositDb");
            dbClientDeposit.Create();
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
            commonSteps.Authorization(loginPhone, password);
            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Депозиты");
            browserSteps.UpdateCurrentPage("DepositPage");
        }

        [AllureStory("Проверка отображения депозитных продуктов на разных валютах")]
        [AllureStep("Проверка отображения депозитных продуктов на разных валютах")]
        [Test]
        public void CheckingDisplayDepositProductsInDifferientCurrenciesTest()
        {
            elementSteps.IsWebElementDisplayed("ДепозитныеПродукты").Should().BeTrue();
           
            var listDepositsRub = DbQueries.GetDepositsInformation(dbClientDeposit, "RUB");
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаSummerOfferRUB", "LabelSummerOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаWinterOfferRUB", "LabelWinterOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаSpringOfferRUB", "LabelSpringOffer", dbColumnDepositsName);

            elementSteps.ClickToWebElement("ДепозитныеПродуктыUSD");
            var listDepositsUSD = DbQueries.GetDepositsInformation(dbClientDeposit, "USD");
            commonSteps.CheckDepositExistsOnPage(listDepositsUSD, "КарточкаUniversalUSD", "LabelUniversal", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsUSD, "КарточкаUSDKeepOfferDepositCard", "LabelKeepOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsUSD, "КарточкаGarantDepositCardUSD", "LabelGarant", dbColumnDepositsName);

            elementSteps.ClickToWebElement("ДепозитныеПродуктыEUR");
            var listDepositsEUR = DbQueries.GetDepositsInformation(dbClientDeposit, "EUR");
            commonSteps.CheckDepositExistsOnPage(listDepositsEUR, "КарточкаKeepOfferEUR", "LabelEuroKeepOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsEUR, "КарточкаClassicEUR", "LabelClassic", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsEUR, "КарточкаClassicPlusEUR", "LabelClassic+", dbColumnDepositsName);
        }

        [AllureStory("Проверка отображения вкладки RUB по дефолту")]
        [AllureStep("Проверка отображения вкладки RUB по дефолту")]
        [Test]
        public void CheckingDisplayOfRUBTabByDefaultTest()
        {
            elementSteps.ClickToWebElement("ДепозитныеПродуктыRUB");
            elementSteps.GetAttributeValue("ДепозитныеПродуктыRUB", "aria-selected").Should().Be("true");
        }

        [TearDown]
        public void TearDown()
        {
            dbClientDeposit.Dispose();
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }
}
