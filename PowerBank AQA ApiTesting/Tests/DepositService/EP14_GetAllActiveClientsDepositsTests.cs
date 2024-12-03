using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.Models;
using FluentAssertions;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_TestingCore.Configuration;
using Newtonsoft.Json.Schema;
using PowerBank_AQA_ApiTestingCore.Extensions;
using PowerBank_AQA_TestingCore.Extensions;
using PowerBank_AQA_ApiTesting.Extensions;
using PowerBank_AQA_DbTestingCore.DbClient;

namespace PowerBank_AQA_ApiTesting.Tests.DepositService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-3.14 Получение всех активных и отображаемых депозитных счетов клиента")]
    [AllureFeature("EP-3.14 Получение всех активных и отображаемых депозитных счетов клиента")]
    [AllureEpic("Deposit service")]
    public class EP14_GetAllActiveClientsDepositsTests
    {
        private WebService service;
        private IConfiguration configuration;
        private string endPoint;
        private readonly string userNumber = "79772345685";
        private readonly string userPassword = "ls23Ghq#wEr";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
        }

        [Test, Order(1)]
        [AllureStory("Проверка всех депозитных счетов клиента")]
        [AllureStep("Проверка всех депозитных счетов клиента")]
        public void GetAllActiveClientsDepositsValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);
            var response = apiMethods.GetAllActiveClientsDeposits(token);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchemaList<OpenDepositInformation>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(2)]
        [AllureStory("Проверка всех депозитных счетов клиента без получения токена")]
        [AllureStep("Проверка всех депозитных счетов клиента без получения токена")]
        public void GetAllActiveClientsDepositsWithoutTokenTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var token = string.Empty;

            var response = apiMethods.GetAllActiveClientsDeposits(token);
            response.StatusCode.CheckStatusCode(401);
        }


        [Test, Order(3)]
        [AllureStory("Проверка всех депозитных счетов клиента у которого нет открытых депозитов")]
        [AllureStep("Проверка всех депозитных счетов клиента у которого нет открытых депозитов")]
        public void GetAllActiveClientsDepositsWhenClientDoesntHaveThemTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            DbClient dbClient;
            dbClient = new DbClient(configuration);
            dbClient.Create();


            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(10),
                Password = string.Empty.CreateRandomStringNumbers(3) + string.Empty.CreateRandomString(3).UpperFirstChar() +
                                 string.Empty.CreateRandomStringSpecialCharacters(),
                SecurityQuestion = string.Empty.CreateRandomString(),
                SecurityAnswer = string.Empty.CreateRandomString(),
                Email = string.Empty.CreateRandomString(9) + "@mail.ru",
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                PassportNumber = string.Empty.CreateRandomStringNumbers(10),
                CountryOfResidence = true
            };
            var response = apiMethods.GetResponseFromPostNewUser(user);

            try
            {
                var userLogin = new LoginUser
                {
                    MobilePhone = user.MobilePhone,
                    PasswordEncode = user.Password

                };
                var token = apiMethods.GetAccessToken(userLogin);
                var responseDeposits = apiMethods.GetAllActiveClientsDeposits(token);
                responseDeposits.StatusCode.CheckStatusCode(404); 
            }
            finally
            {
                DbQueries.DeleteUser(user, dbClient);
                dbClient.Dispose();
            }
        }

        [Test, Order(4)]
        [AllureStory("Не валидный запрос всех активных и отображаемых депозитных счетов клиента")]
        [AllureStep("Не валидный запрос всех активных и отображаемых депозитных счетов клиента")]
        public void GetAllActiveClientsDepositsWrongEndpointTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);
            endPoint = configuration["BaseUrl"].ToString() + "api/v1/deposittttttt/accounts";

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Get, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            response.StatusCode.CheckStatusCode(404);
        }

        [Test, Order(5)]
        [AllureStory("Проверка всех депозитных счетов клиента c искажённым токеном")]
        [AllureStep("Проверка всех депозитных счетов клиента c искажённым токеном")]
        public void GetAllActiveClientsDepositsWrongTokenTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user) + string.Empty.CreateRandomString(1);
            var response = apiMethods.GetAllActiveClientsDeposits(token);
            response.StatusCode.CheckStatusCode(401);
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }
    }
}
