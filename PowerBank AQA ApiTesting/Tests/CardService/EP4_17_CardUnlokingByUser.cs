using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_TestingCore.Configuration;

namespace PowerBank_AQA_ApiTesting.Tests.CardService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-4.17 Разблокировка карты пользователем")]
    [AllureFeature("EP-4.17 Разблокировка карты пользователем")]
    [AllureEpic("Card service")]
    public class EP4_17_CardUnlokingByUser
    {
        private WebService service;
        private IConfiguration configuration;
        private DbClient dbClientCard;
        private string token;
        private readonly string userPhoneNumber = "79772345685";
        private readonly string userPassword = "ls23Ghq#wEr";
        private readonly string cardNumber = "5123459569776203";
        private readonly string someoneElseCardNumber = "9999050500003055";
        private readonly string nonExistentCardNumber = "5555777766660000";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser()
            {
                MobilePhone = userPhoneNumber,
                PasswordEncode = userPassword
            };
            token = apiMethods.GetAccessToken(user);
        }

        [AllureStory("Разблокировка карты пользователем")]
        [AllureStep("Разблокировка карты пользователем")]
        [Test]
        public void CardUnlockingByUserTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClientCard = new DbClient(configuration, "CardDb");
            dbClientCard.Create();
            
            var responseFromCardLocking = apiMethods.LockingCard(token, cardNumber);
            responseFromCardLocking.StatusCode.CheckStatusCode(200);
            DbQueries.IsUserBlockedCard(dbClientCard, cardNumber).Should().Be(true);
            var responseFromCardUnlocking = apiMethods.UnlockingCard(token, cardNumber);
            responseFromCardUnlocking.StatusCode.CheckStatusCode(200);
            DbQueries.IsUserBlockedCard(dbClientCard, cardNumber).Should().Be(false);
            apiMethods.RecreatingDatabaseCard();
        }

        [AllureStory("Ввод невалидных значений CardNumber")]
        [AllureStep("Ввод невалидных значений CardNumber")]
        [TestCase("55557777666699")]
        [TestCase("55557777666699990000")]
        [TestCase("55!!777766669999")]
        [TestCase(" ")]
        public void CardUnlockingByUserWithInvalidCardNumberTest(string invalidCardNumber)
        {
            ApiMethods apiMethods = new ApiMethods();
            var responseFromCardLoking = apiMethods.LockingCard(token, cardNumber);
            responseFromCardLoking.StatusCode.CheckStatusCode(200);
            var responseFromCardUnlocking = apiMethods.UnlockingCard(token, invalidCardNumber);
            responseFromCardUnlocking.StatusCode.CheckStatusCode(400);
            apiMethods.RecreatingDatabaseCard();
        }

        [AllureStory("Разблокировка карты неавторизованным пользователем")]
        [AllureStep("Разблокировка карты неавторизованным пользователем")]
        [Test]
        public void CardUnlockingByUnauthorizedUser() 
        {
            ApiMethods apiMethods = new ApiMethods();
            var responseFromCardUnlocking = apiMethods.UnlockingCard(string.Empty, cardNumber);
            responseFromCardUnlocking.StatusCode.CheckStatusCode(401);
        }

        [AllureStory("Разблокировка карты с чужим токеном авторизации")]
        [AllureStep("Разблокировка карты с чужим токеном авторизации")]
        [Test]
        public void CardUnlockingWithSomeoneElseToken()
        {
            ApiMethods apiMethods = new ApiMethods();
            var responseFromCardLocking = apiMethods.LockingCard(token, cardNumber);
            responseFromCardLocking.StatusCode.CheckStatusCode(200);
            var responseFromCardUnlocking = apiMethods.UnlockingCard(token, someoneElseCardNumber);
            responseFromCardUnlocking.StatusCode.CheckStatusCode(404);
            apiMethods.RecreatingDatabaseCard();
        }

        [AllureStory("Ввод несуществующего Card Number")]
        [AllureStep("Ввод несуществующего Card Number")]
        [Test]
        public void CardUnlockWithNonExistentCardNumber()
        {
            ApiMethods apiMethods = new ApiMethods();
            var responseFromCardLocking = apiMethods.LockingCard(token, cardNumber);
            responseFromCardLocking.StatusCode.CheckStatusCode(200);
            var responseFromCardUnlocking = apiMethods.UnlockingCard(token, nonExistentCardNumber);
            responseFromCardUnlocking.StatusCode.CheckStatusCode(404);
            apiMethods.RecreatingDatabaseCard();
        }

        public void TearDown()
        {
            service.Dispose();
        }
    }
}
