using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Extensions;

namespace PowerBank_AQA_ApiTesting.Tests.TransactionHistoryService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP 6.1 Просмотр истории операций по карте")]
    [AllureFeature("EP 6.1 Просмотр истории операций по карте")]
    [AllureEpic("Info service")]
    public class EP6_1_TransactionHistoryCardTests
    {
        private WebService service;
        private IConfiguration configuration;
        private string accountNumber = "1111111114";
        private readonly string userNumber = "79772345685";
        private readonly string userPassword = "ls23Ghq#wEr";
        private readonly string startTime = "2022-07-06";
        private readonly string endTime = "2023-08-06";
        private readonly string userNumberFalse = "79990234561";
        private readonly string userPasswordFalse = "aqwQQport@43";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
        }

        [Test, Order(1)]
        [AllureStory("История операций за период")]
        [AllureStep("История операций за период")]
        public void CheckTransactionHistoryDateTimeSearchValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 10,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = string.Empty,
                TransactionMarker = 0
            };

            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(2)]
        [AllureStory("Поиск по наименованию операции")]
        [AllureStep("Поиск по наименованию операции")]
        public void CheckTransactionHistoryTransactionTypeValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 10,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = "Перевод",
                TransactionMarker = 0
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(3)]
        [AllureStory("Поиск по некорректному наименованию операции")]
        [AllureStep("Поиск по некорректному наименованию операции")]
        public void CheckTransactionHistoryNotExistingTransactionTypeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 5,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = string.Empty.CreateRandomString(5),
                TransactionMarker = 0
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(4)]
        [AllureStory("Поиск по назначению операции")]
        [AllureStep("Поиск по назначению операции")]
        public void CheckTransactionHistoryTransactionTypeAnotherValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 10,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = "",
                TransactionMarker = 0
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");

            cardTransactionHistory.TransactionMarker = 1;
            response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");

            cardTransactionHistory.TransactionMarker = 2;
            response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(5)]
        [AllureStory("Поиск по некорректному назначению операции")]
        [AllureStep("Поиск по некорректному назначению операции")]
        public void CheckTransactionHistoryWrongTransactionTypeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 5,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = "",
                TransactionMarker = -4
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(6)]
        [AllureStory("Отображение расходных и приходных операций")]
        [AllureStep("Отображение расходных и приходных операций")]
        public void CheckTransactionHistoryReplenishmentValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 10,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = string.Empty,
                TransactionMarker = 1
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(7)]
        [AllureStory("Отображение свежих операций по карте")]
        [AllureStep("Отображение свежих операций по карте")]
        public void CheckTransactionHistoryNewestValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 5,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = string.Empty,
                TransactionMarker = 1
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(8)]
        [AllureStory("Просмотр истории операций по карте c токеном чужого юзера")]
        [AllureStep("Просмотр истории операций по карте c токеном чужого юзера")]
        public void CheckTransactionHistoryNotValidTokenTest()
        {
            ApiMethods apiMethods = new ApiMethods();

            var user = new LoginUser
            {
                MobilePhone = userNumberFalse,
                PasswordEncode = userPasswordFalse
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 10,
                AccountNumber = accountNumber,
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = "123",
                TransactionMarker = 0
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [Test, Order(9)]
        [AllureStory("Просмотр истории операций по карте для другого accountNumber")]
        [AllureStep("Просмотр истории операций по карте для другого accountNumber")]
        public void CheckTransactionHistoryOthersAccountNumberTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var cardTransactionHistory = new CardTransactionHistory
            {
                Page = 0,
                PerPage = 10,
                AccountNumber = "1111111116",
                StartDate = startTime,
                EndDate = endTime,
                TransactionType = string.Empty,
                TransactionMarker = 0
            };
            var response = apiMethods.CheckTransactionsHistoryCard(token, cardTransactionHistory);
            response.StatusCode.CheckStatusCode(200);
            var schema = SchemaGenerator.CreateSchema<CardTransactionHistoryResponseModel>();
            var responseMessage = response.ResponseBody;
            responseMessage.IsValid(schema).Should().Be(true, "Не пришел ожидаемый результат");
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }
    }
}
