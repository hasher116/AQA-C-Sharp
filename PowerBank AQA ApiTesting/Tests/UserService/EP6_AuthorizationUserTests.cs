using Microsoft.Extensions.Configuration;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTestingCore.ApiRequest;
using PowerBank_AQA_ApiTestingCore.Models;
using System.Net;
using FluentAssertions;
using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Extensions;
using NUnit.Allure.Core;
using NUnit.Allure.Attributes;
using PowerBank_AQA_ApiTesting.Extensions;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTesting.AllureSteps;

namespace PowerBank_AQA_ApiTesting.Tests.UserService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-6 Авторизация Пользователя по Номеру телефона/Номеру паспорта ")]
    [AllureFeature("EP-6 Авторизация Пользователя по Номеру телефона/Номеру паспорта ")]
    [AllureEpic("User service")]
    public class EP6_AuthorizationUserTests
    {
        private WebService service;
        private IConfiguration configuration;
        private string loginPathPhone;
        private string loginPathPassport;
        private readonly string endPointPhone = "api/v1/user/login/mobile_phone";
        private readonly string endPointPassport = "api/v1/user/login/passport_number";
        private DbController dbController;
        private DbClient dbClient;
        ApiMethods apiMethods = new ApiMethods();

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
            loginPathPhone = configuration["BaseUrl"].ToString() + endPointPhone;
            loginPathPassport = configuration["BaseUrl"].ToString() + endPointPassport;
            dbController = new DbController();
            dbClient = new DbClient(configuration);
            dbClient.Create();
            dbController.Connections.TryAdd("1", dbClient);
        }

        [Test, Order(1)]
        [AllureStory("Login with phone valid data")]
        [AllureStep("Login with phone valid data")]
        public void AuthorizationPhoneValidDataTest()
        {
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

            var userLogin = new LoginUser
            {
                MobilePhone = user.MobilePhone,
                PasswordEncode = user.Password
            };

            var headers = RequestCreator.CreateHeaders(HeaderTypes.JSON);
            var request = RequestCreator.CreateRequest(userLogin, headers, loginPathPhone, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var responseFromLogin = service.SendMessageAsync(request).Result;
            responseFromLogin.StatusCode.CheckStatusCode(200);
            DbQueries.DeleteUser(user, dbClient);
        }

        [Test, Order(2)]
        [AllureStory("Login with passport valid data")]
        [AllureStep("Login with passport valid data")]
        public void AuthorizationPassportValidDataTest()
        {
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

            var userLogin = new LoginUser
            {
                PassportNumber = user.PassportNumber,
                PasswordEncode = user.Password
            };

            try
            {
                var headers = RequestCreator.CreateHeaders(HeaderTypes.JSON);
                var request = RequestCreator.CreateRequest(userLogin, headers, loginPathPassport, HttpMethod.Post, int.Parse(configuration["timeOut"]));
                var responseFromLogin = service.SendMessageAsync(request).Result;
                responseFromLogin.StatusCode.CheckStatusCode(200);
            }
            finally
            {
                DbQueries.DeleteUser(user, dbClient);
            }
        }

        [Test, Order(3)]
        [AllureStory("Login with passport wrong passport")]
        [AllureStep("Login with passport wrong passport")]
        public void AuthorizationPassportWrongPasswordTest()
        {
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

            var userLogin = new LoginUser
            {
                PassportNumber = user.PassportNumber,
                PasswordEncode = string.Empty.CreateRandomString()
            };
            try
            {
                var headers = RequestCreator.CreateHeaders(HeaderTypes.JSON);
                var request = RequestCreator.CreateRequest(userLogin, headers, loginPathPassport, HttpMethod.Post, int.Parse(configuration["timeOut"]));
                var responseFromLogin = service.SendMessageAsync(request).Result;
                responseFromLogin.StatusCode.CheckStatusCode(400);
            }
            finally
            {
                DbQueries.DeleteUser(user, dbClient);
            }
        }

        [TearDown]
        public void TearDown()
        {
            dbClient.Dispose();
            service.Dispose();
            dbController.Connections.Clear();
        }
    }
}
