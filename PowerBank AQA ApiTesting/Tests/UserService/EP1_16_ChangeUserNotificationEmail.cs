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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBank_AQA_ApiTesting.Tests.UserService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-1.16 Включение и отключение email-рассылки")]
    [AllureFeature("EP-1.16 Включение и отключение email-рассылки")]
    [AllureEpic("User service")]
    public class EP1_16_ChangeUserNotificationEmail
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

        [AllureStory("Отключение рассылки")]
        [AllureStep("Отключение рассылки ")]
        [Test, Order(1)]
        public void DisableNotification()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClient = new DbClient(configuration);
            dbClient.Create();

            var notification = new Notification()
            {
                NotificationStatus = false
            };

            var response = apiMethods.ChangeNotification(token, notification);
            response.StatusCode.CheckStatusCode(200);
            DbQueries.GetNotification(dbClient).Should().BeFalse();
        }

        [AllureStory("Включение рассылки")]
        [AllureStep("Включение рассылки")]
        [Test, Order(2)]
        public void EnableNotification()
        {
            ApiMethods apiMethods = new ApiMethods();
            dbClient = new DbClient(configuration);
            dbClient.Create();

            var notification = new Notification()
            {
                NotificationStatus = true
            };

            var response = apiMethods.ChangeNotification(token, notification);
            response.StatusCode.CheckStatusCode(200);
            DbQueries.GetNotification(dbClient).Should().BeTrue();
        }

        [AllureStory("Пользователь не авторизован")]
        [AllureStep("Пользователь не авторизован")]
        [Test, Order(3)]
        public void UserUnauthorizedNotification()
        {
            ApiMethods apiMethods = new ApiMethods();
            var notification = new Notification()
            {
                NotificationStatus = true
            };

            var response = apiMethods.ChangeNotification(null, notification);
            response.StatusCode.CheckStatusCode(401);
        }
    }
}
