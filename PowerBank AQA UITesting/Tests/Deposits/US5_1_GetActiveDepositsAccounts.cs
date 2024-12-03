using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Hooks;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;
using System.Globalization;

namespace PowerBank_AQA_UITesting.Tests.Deposits
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US5_1 Просмотр заключенных депозитных договоров")]
    [AllureFeature("US5_1 Просмотр заключенных депозитных договоров")]
    [AllureEpic("US5_1 Просмотр заключенных депозитных договоров")]
    public class US5_1_GetActiveDepositsAccounts : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;
        private DbClient dbClientDeposit;
        private DbClient dbClient;
        private readonly string loginPhoneWithActiveDeposits = "9772345685";
        private readonly string passwordWithActiveDeposits = "ls23Ghq#wEr";
        private readonly string loginPhoneDoesntHaveDeposits = "9213471234";
        private readonly string passwordDoesntHaveDeposits = "12Zaq@ERTlop";
        private readonly string dbColumnDepositsName = "Name";

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            dbClientDeposit = new DbClient(configuration, "DepositDb");
            dbClientDeposit.Create();
            dbClient = new DbClient(configuration);
            dbClient.Create();
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
        }

        [AllureStory("Просмотр действующих депозитов")]
        [AllureStep("Просмотр действующих депозитов")]
        [Test, Order(1)]
        public void ViewingActiveDepositsTest()
        {
            commonSteps.Authorization(loginPhoneDoesntHaveDeposits, passwordDoesntHaveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.IsWebElementNotDisplayed("МоиДепозиты").Should().BeTrue();

            var listDepositsRub = DbQueries.GetDepositsInformation(dbClientDeposit, "RUB");
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаSummerOfferRUB", "LabelSummerOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаWinterOfferRUB", "LabelWinterOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаSpringOfferRUB", "LabelSpringOffer", dbColumnDepositsName);

            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Выход");

            browserSteps.UpdateCurrentPage("LoginPage");
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.IsWebElementDisplayed("МоиДепозиты").Should().BeTrue();
      
            commonSteps.CheckOpenedDepositInformation("КарточкаWinterOfferRUBUserHave",browserSteps);
            commonSteps.CheckOpenedDepositInformation("КарточкаSummerOfferRUBUserHave", browserSteps);
        }

        [AllureStory("Switcher \"Автоматическое продление\"")]
        [AllureStep("Switcher \"Автоматическое продление\"")]
        [Test, Order(2)]
        public void SwitcherAutomaticRenewalTest()
        {
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.ClickToWebElement("КарточкаSummerOfferRUBUserHave");
            browserSteps.UpdateCurrentPage("OpenedDepositPage");

            elementSteps.ClickToWebElement("Свитчер Автопродление включить");
            elementSteps.IsWebElementDisplayed("Свитчер Автопродление неактивный").Should().BeTrue();
            elementSteps.ClickToWebElement("Свитчер Автопродление включить");
            elementSteps.IsWebElementDisplayed("Свитчер Автопродление активный").Should().BeTrue();
        }

        [AllureStory("Отображение информации у пользователя, имеющего активный/е депозит/ы")]
        [AllureStep("Отображение информации у пользователя, имеющего активный/е депозит/ы")]
        [Test, Order(3)]
        public void ViewingActiveDepositsFromUserTest()
        {
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.IsWebElementDisplayed("МоиДепозиты").Should().BeTrue();

            commonSteps.CheckLegalAddressAndPhone();
        }

        [AllureStory("Отображение карточек действующих депозитов пользователя")]
        [AllureStep("Отображение карточек действующих депозитов пользователя")]
        [Test, Order(4)]
        public void ViewingCardsWhenUserHasActiveDepositsTest()
        {
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.IsWebElementDisplayed("КарточкаSummerOfferRUBUserHave").Should().BeTrue();
            elementSteps.IsWebElementDisplayed("КарточкаWinterOfferRUBUserHave").Should().BeTrue();
        }

        [AllureStory("Отображение развернутой информации по депозиту")]
        [AllureStep("Отображение развернутой информации по депозиту")]
        [Test, Order(5)]
        public void ViewingFullInformationAboutDepositTest()
        {
            browserSteps.UpdateCurrentPage("LoginPage");
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.IsWebElementDisplayed("МоиДепозиты").Should().BeTrue();

            elementSteps.ClickToWebElement("КарточкаSummerOfferRUBUserHave");
        }

        [AllureStory("Кнопка \"Отозвать депозит\"")]
        [AllureStep("Кнопка \"Отозвать депозит\"")]
        [Test, Order(6)]
        public void WithdrawDepositButtonTest()
        {
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);
            commonSteps.GoToDepositPage(browserSteps);
            elementSteps.ClickToWebElement("КарточкаSummerOfferRUBUserHave");
            browserSteps.UpdateCurrentPage("OpenedDepositPage");
            elementSteps.ClickToWebElement("Отозвать депозит");
            elementSteps.IsWebElementDisplayed("Список выбора карт после 'Отозвать депозит'").Should().BeTrue();
        }

        [AllureStory("Отображение информации у пользователя, не имеющего оформленный депозит")]
        [AllureStep("Отображение информации у пользователя, не имеющего оформленный депозит")]
        [Test, Order(7)]
        public void ViewingDepositsUserDoesntHaveItTest()
        {
            commonSteps.Authorization(loginPhoneDoesntHaveDeposits, passwordDoesntHaveDeposits);

            commonSteps.GoToDepositPage(browserSteps);

            elementSteps.IsWebElementNotDisplayed("МоиДепозиты").Should().BeTrue();

            var listDepositsRub = DbQueries.GetDepositsInformation(dbClientDeposit, "RUB");
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаSummerOfferRUB", "LabelSummerOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаWinterOfferRUB", "LabelWinterOffer", dbColumnDepositsName);
            commonSteps.CheckDepositExistsOnPage(listDepositsRub, "КарточкаSpringOfferRUB", "LabelSpringOffer", dbColumnDepositsName);

            commonSteps.CheckLegalAddressAndPhone();

            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Выход");
        }

        [AllureStory("Ссылка на документ \"Условия депозитного продукта\"")]
        [AllureStep("Ссылка на документ \"Условия депозитного продукта\"")]
        [Test, Order(8)]
        public void CheckLinkToTheDocumentTermsDepositProductTest()
        {
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);
            commonSteps.CheckDepositConditions(browserSteps, "КарточкаSummerOfferRUBUserHave", "Форма договора");
        }

        [AllureStory("Ссылка на документ \"Общие условия размещения средств в депозит\"")]
        [AllureStep("Ссылка на документ \"Общие условия размещения средств в депозит\"")]
        [Test, Order(9)]
        public void CheckLinkToTheDocumentGeneralConditionsForPlacingFundsOnDepositTest()
        {
            commonSteps.Authorization(loginPhoneWithActiveDeposits, passwordWithActiveDeposits);

            commonSteps.GoToDepositPage(browserSteps);
            commonSteps.CheckDepositConditions(browserSteps, "КарточкаSummerOfferRUBUserHave", "Условия депозитного продукта");
        }

        [TearDown]
        public void TearDown()
        {
            dbClient.Dispose();
            dbClientDeposit.Dispose();
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }
}
