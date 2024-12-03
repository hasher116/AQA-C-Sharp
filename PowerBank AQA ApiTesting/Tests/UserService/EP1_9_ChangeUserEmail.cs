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
using PowerBank_AQA_TestingCore.Extensions;

namespace PowerBank_AQA_ApiTesting.Tests.UserService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-1.9 Изменение email клиента")]
    [AllureFeature("EP-1.9 Изменение email клиента")]
    [AllureEpic("User service")]
    public class EP1_9_ChangeUserEmail
    {
        private WebService service;
        private IConfiguration configuration;
        private DbClient dbClient;
        private string token;
        private readonly string userPhoneNumber = "79772345685";
        private readonly string userPassword = "ls23Ghq#wEr";

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

        [AllureStory("Изменение email")]
        [AllureStep("Изменение email")]
        [Test]
        public void ChangeEmailByUserTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClient = new DbClient(configuration);
            dbClient.Create();

            var email = new Email()
            {
                newEmail = string.Empty.CreateRandomString(7) + "@mail.ru"
            };

            var response = apiMethods.ChangeEmail(token, email);
            response.StatusCode.CheckStatusCode(200);
            DbQueries.GetEmail(dbClient, userPhoneNumber).Should().BeEquivalentTo(email.newEmail);
            apiMethods.RecreatingDatabaseUsers();
        }

        [AllureStory("Изменение email без авторизации")]
        [AllureStep("Изменение email без авторизации")]
        [Test]
        public void ChangeEmailWithoutAuthorizationTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClient = new DbClient(configuration);
            dbClient.Create();

            var email = new Email()
            {
                newEmail = string.Empty.CreateRandomString(7) + "@mail.ru"
            };

            var response = apiMethods.ChangeEmail(null, email);
            response.StatusCode.CheckStatusCode(401);
            apiMethods.RecreatingDatabaseUsers();
        }

        [AllureStory("Невалидный email")]
        [AllureStep("Невалидный email")]
        [Test]
        public void ChangeEmailNotValid()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClient = new DbClient(configuration);
            dbClient.Create();

            var email = new Email()
            {
                newEmail = string.Empty.CreateRandomString(7)
            };

            var response = apiMethods.ChangeEmail(token, email);
            response.StatusCode.CheckStatusCode(400);
            apiMethods.RecreatingDatabaseUsers();
        }

        [AllureStory("Пустое Значение email")]
        [AllureStep("Пустое Значение email")]
        [Test]
        public void ChangeEmailToNull()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClient = new DbClient(configuration);
            dbClient.Create();

            var email = new Email()
            {
                newEmail = string.Empty
            };

            var response = apiMethods.ChangeEmail(token, email);
            response.StatusCode.CheckStatusCode(400);
            apiMethods.RecreatingDatabaseUsers();
        }
    }
}
