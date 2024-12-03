using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Dto;
using PowerBank_AQA_UITesting.Hooks;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;

namespace PowerBank_AQA_UITesting.Tests.RegistrationAutomationSecurity
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US1_8 Курсы валют")]
    [AllureFeature("US1_8 Курсы валют")]
    [AllureEpic("US1_8 Курсы валют")]
    public class US1_8_ExchangeRates : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private MongoDbClient dbClient;
        private List<BsonDocument> documents;
        private IConfiguration configuration;
        private readonly string loginPhone = "9772345685";
        private readonly string password = "ls23Ghq#wEr";

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            dbClient = new MongoDbClient(configuration, "MongoInfo");
            dbClient.Create();
            var collection = dbClient.GetCollection<BsonDocument>("ExchangeRates");
            documents = dbClient.GetDocuments(collection);
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
        }

        [AllureStory("Просмотр курса валют зарегистрированным пользователем")]
        [AllureStep("Просмотр курса валют зарегистрированным пользователем")]
        [Test, Order(1)]
        public void CheckingExchangeRatesByAuthorizatedUser()
        {
            commonSteps.Authorization(loginPhone, password);
            browserSteps.UpdateCurrentPage("Header");
            elementSteps.ClickToWebElement("Курсы валют");
            browserSteps.UpdateCurrentPage("ExchangeRatesPage");

            MongoDbHelper mongoDbHelper = new MongoDbHelper();
            List<ExchangeRate> expectedResults = mongoDbHelper.ConvertBsonToListExchangeRate(documents);
            List<ExchangeRate> actualResults = commonSteps.GetExchangeRates();

            commonSteps.CompareExchangeRates(expectedResults, actualResults);
        }

        [AllureStory("Просмотр курса валют незарегистрированным пользователем")]
        [AllureStep("Просмотр курса валют незарегистрированным пользователем")]
        [Test, Order(2)]
        public void CheckingExchangeRatesByUnauthorizatedUser()
        {
            browserSteps.UpdateCurrentPage("Header");
            elementSteps.ClickToWebElement("Курсы валют");
            browserSteps.UpdateCurrentPage("ExchangeRatesPage");

            MongoDbHelper mongoDbHelper = new MongoDbHelper();
            List<ExchangeRate> expectedResults = mongoDbHelper.ConvertBsonToListExchangeRate(documents);
            List<ExchangeRate> actualResults = commonSteps.GetExchangeRates();

            commonSteps.CompareExchangeRates(expectedResults, actualResults);
        }

        [TearDown]
        public void TearDown()
        {
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }
}
