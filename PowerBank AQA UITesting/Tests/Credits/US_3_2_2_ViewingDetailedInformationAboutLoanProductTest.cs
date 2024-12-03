using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_UITesting.Steps;
using PowerBank_AQA_UITestingCore.Models.PageObject;
using PowerBank_AQA_UITestingCore.Models.Settings;

namespace PowerBank_AQA_UITesting.Tests.Credits
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("US_3.2.2 Просмотр детальной информации о кредитном продукте")]
    [AllureFeature("US_3.2.2 Просмотр детальной информации о кредитном продукте")]
    [AllureEpic("US_3.2.2 Просмотр детальной информации о кредитном продукте")]
    public class US_3_2_2_ViewingDetailedInformationAboutLoanProductTest : BaseTest
    {
        private BrowserSteps browserSteps;
        private ElementSteps elementSteps;
        private CommonSteps commonSteps;
        private IConfiguration configuration;
        private MongoDbClient dbClient;
        private List<BsonDocument> documents;
        private readonly string loginPhone = "9772345685";
        private readonly string password = "ls23Ghq#wEr";

        [SetUp]
        public void Setup()
        {
            configuration = Configuration.GetConfiguration();
            dbClient = new MongoDbClient(configuration, "MongoCredit");
            dbClient.Create();
            var collection = dbClient.GetCollection<BsonDocument>("creditProduct");
            documents = dbClient.GetDocuments(collection);
            browserSteps = new BrowserSteps(settingsContainer.Resolve<Settings>(), pageContainer.Resolve<IEnumerable<Node>>());
            browserSteps.StartBrowser();
            browserSteps.SetCurrentPage("LoginPage");
            elementSteps = new ElementSteps(browserSteps.GetBrowser(), settingsContainer.Resolve<Settings>());
            commonSteps = new CommonSteps(browserSteps.GetBrowser(), elementSteps, settingsContainer.Resolve<Settings>());
            commonSteps.Authorization(loginPhone, password);
            browserSteps.UpdateCurrentPage("LeftMenu");
            elementSteps.ClickToWebElement("Кредиты");
            browserSteps.UpdateCurrentPage("LoansPage");
        }

        [AllureStory("Получить детальную информацию о кредите")]
        [AllureStep("Получить детальную информацию о кредите")]
        [Test, Order(1)]
        public void GetDetailedInformationAboutLoanProductTest()
        {
            commonSteps.CheckLoansInformation(browserSteps, "Подробнее кредит классический", documents[0]);

            commonSteps.CheckLoansInformation(browserSteps, "Подробнее кредит срочный", documents[1]);

            commonSteps.CheckLoansInformation(browserSteps, "Подробнее кредит авто", documents[2]);
        }

        [AllureStory("Открыть PDF-файл с условиями кредита")]
        [AllureStep("Открыть PDF-файл с условиями кредита")]
        [Test, Order(2)]
        public void OpenPDFFileWithLoansConditionsTest()
        {
            commonSteps.CheckLoansConditions(browserSteps, "Подробнее кредит классический", "Условие кредитного договора Классический");

            commonSteps.CheckLoansConditions(browserSteps, "Подробнее кредит срочный", "Условие кредитного договора Срочный");

            commonSteps.CheckLoansConditions(browserSteps, "Подробнее кредит авто", "Условие кредитного договора Авто");
        }

        [TearDown]
        public void TearDown()
        {
            browserSteps.CloseWebPage();
            browserSteps.CloseBrowser();
        }
    }
}
