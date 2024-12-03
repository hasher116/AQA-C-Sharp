using FluentAssertions;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Extensions;
using PowerBank_AQA_ApiTestingCore.ApiRequest;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_TestingCore.Configuration;
using Microsoft.Extensions.Configuration;
using System.Net;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_DbTestingCore.DbHelper;
using PowerBank_AQA_ApiTesting.Hooks;
using System.Data;
using AutoMapper;
using PowerBank_AQA_TestingCore.Extensions;
using NUnit.Allure.Core;
using NUnit.Allure.Attributes;
using PowerBank_AQA_ApiTestingCore.Helpers;
using Allure.Commons;
using PowerBank_AQA_ApiTesting.AllureSteps;

namespace PowerBank_AQA_ApiTesting.Tests.UserService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-5 Регистрация не Клиента в Приложении ")]
    [AllureFeature("EP-5 Регистрация не Клиента в Приложении ")]
    [AllureEpic("User service")]
    public class EP_5_RegistrationNewUserTests
    {
        private WebService service;
        private IConfiguration configuration;
        private DbController dbController;
        private DbClient dbClient;
        private string registrationPath;
        private readonly string endPoint = "api/v1/user/registration/user-profile/new";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
            dbController = new DbController();
            dbClient = new DbClient(configuration);
            dbClient.Create();
            dbController.Connections.TryAdd("1", dbClient);
            registrationPath = configuration["BaseUrl"].ToString() + endPoint;
        }

        [Test, Order(1)]
        [AllureStory("Registration new user with valid data")]
        [AllureStep("Registration new user with valid data")]
        public void RegistrationNewUserValidDataTest()
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
            ApiMethods apiMethods = new ApiMethods();
            var response = apiMethods.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(200);

            var rowFromTable = dbClient.SelectOneRow("SELECT * FROM public.\"UserProfiles\" WHERE \"Email\" = '" + user.Email + "' AND \"PasswordEncode\" = '" + user.Password + "'");
            var mapper = MyMapper.NewUser.CreateMapper();
            var userFromDB = mapper.Map<DataRow, RegistrationNewUser>(rowFromTable);
            userFromDB.Id.Should().NotBeNull("Пользователь не был создан");
            DbQueries.DeleteUser(user, dbClient);
        }

        [Test, Order(2)]
        [AllureStory("Registration new user with wrong phone")]
        [AllureStep("Registration new user with wrong phone")]
        public void RegistrationNewUserWrongPhoneTest()
        {
            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(11),
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
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Mobile phone must have 11 digits without +", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(3)]
        [AllureStory("Registration new user with wrong password")]
        [AllureStep("Registration new user with wrong password")]
        public void RegistrationNewUserWrongPasswordTest()
        {
            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(10),
                Password = string.Empty.CreateRandomStringNumbers(9),
                SecurityQuestion = string.Empty.CreateRandomString(),
                SecurityAnswer = string.Empty.CreateRandomString(),
                Email = string.Empty.CreateRandomString(9) + "@mail.ru",
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                PassportNumber = string.Empty.CreateRandomStringNumbers(10),
                CountryOfResidence = true
            };
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Password must be from 6 to 20 characters, contain any of the follow characters (!\\\"#$%&'()*+,-./:;<=>?@[\\\\]^_`{{|}}~),have at least one capital letter and at least one digit", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(4)]
        [AllureStory("Registration new user with without mail")]
        [AllureStep("Registration new user with without mail")]
        public void RegistrationNewUserWithoutMailTest()
        {
            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(10),
                Password = string.Empty.CreateRandomStringNumbers(9),
                SecurityQuestion = string.Empty.CreateRandomString(),
                SecurityAnswer = string.Empty.CreateRandomString(),
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                PassportNumber = string.Empty.CreateRandomStringNumbers(10),
                CountryOfResidence = true
            };
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Email' must not be empty.", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(5)]
        [AllureStory("Registration new user with without password")]
        [AllureStep("Registration new user with without password")]
        public void RegistrationNewUserWithoutPasswordTest()
        {
            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(10),
                SecurityQuestion = string.Empty.CreateRandomString(),
                SecurityAnswer = string.Empty.CreateRandomString(),
                Email = string.Empty.CreateRandomString(9) + "@mail.ru",
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                PassportNumber = string.Empty.CreateRandomStringNumbers(10),
                CountryOfResidence = true
            };
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"'Password' must not be empty.", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(6)]
        [AllureStory("Registration new user with without security question")]
        [AllureStep("Registration new user with without security question")]
        public void RegistrationNewUserWithoutSecurityQuestionTest()
        {
            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(10),
                Password = string.Empty.CreateRandomStringNumbers(9),
                SecurityAnswer = string.Empty.CreateRandomString(),
                Email = string.Empty.CreateRandomString(9) + "@mail.ru",
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                PassportNumber = string.Empty.CreateRandomStringNumbers(10),
                CountryOfResidence = true
            };
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Security Question' must not be empty", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(7)]
        [AllureStory("Registration new user with without phone")]
        [AllureStep("Registration new user with without phone")]
        public void RegistrationNewUserWithoutPhoneTest()
        {
            var user = new RegistrationNewUser
            {
                Password = string.Empty.CreateRandomStringNumbers(9),
                SecurityQuestion = string.Empty.CreateRandomString(),
                SecurityAnswer = string.Empty.CreateRandomString(),
                Email = string.Empty.CreateRandomString(9) + "@mail.ru",
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                PassportNumber = string.Empty.CreateRandomStringNumbers(10),
                CountryOfResidence = true
            };
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"'Mobile Phone' must not be empty.", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(8)]
        [AllureStory("Registration new user with without passport")]
        [AllureStep("Registration new user with without passport")]
        public void RegistrationNewUserWithoutPassportTest()
        {
            var user = new RegistrationNewUser
            {
                MobilePhone = "7" + string.Empty.CreateRandomStringNumbers(10),
                Password = string.Empty.CreateRandomStringNumbers(9),
                SecurityQuestion = string.Empty.CreateRandomString(),
                SecurityAnswer = string.Empty.CreateRandomString(),
                Email = string.Empty.CreateRandomString(9) + "@mail.ru",
                FirstName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                LastName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                MiddleName = string.Empty.CreateRandomStringRusLetters(9).UpperFirstChar(),
                CountryOfResidence = true
            };
            ApiMethods usersWork = new ApiMethods();
            var response = usersWork.GetResponseFromPostNewUser(user);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Passport Number' must not be empty.", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [TearDown]
        public void TearDown()
        {
            dbClient.Dispose();
            dbController.Connections.Clear();
            service.Dispose();
        }
    }
}
