using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.ApiRequest;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Extensions;
using System.Data;
using System.Net;

namespace PowerBank_AQA_ApiTesting.Tests.DepositService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-3.15 Полное снятие средств по истечении срока размещения")]
    [AllureFeature("EP-3.15 Полное снятие средств по истечении срока размещения")]
    [AllureEpic("Deposit service")]
    public class EP15_FullWithdrawalTests
    {
        private WebService service;
        private IConfiguration configuration;
        private DbController dbController;
        private DbClient dbClient;
        private DbClient dbClientDeposit;
        private DbClient dbClientCard;
        private DbClient dbClientTransactionHistory;
        private string fullWithdrawalPath;
        private string depositProductId;
        private string clientId;
        private string accountNumber;
        private string cardNumber;
        private double depositBalanceDbAfterOperation;
        private readonly string endPoint = "api/v1/deposit/accounts/fullwithdrawal/";
        private readonly string depositProductName = "SummerOffer";
        private readonly string userEmail = "korneychyk20@fake.ru";
        private readonly string userName = "Semyon Korneichuk";
        private readonly string accountNumberForTransaction = "1111111116";
        private readonly string userMobilePhone = "79990234561";
        private readonly string userPassword = "aqwQQport@43";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
            dbController = new DbController();
            dbClient = new DbClient(configuration);
            dbClient.Create();
            dbClientDeposit = new DbClient(configuration, "DepositDb");
            dbClientDeposit.Create();
            dbClientCard = new DbClient(configuration, "CardDb");
            dbClientCard.Create();
            dbClientTransactionHistory = new DbClient(configuration, "TransactionHistoryDb");
            dbClientTransactionHistory.Create();
            fullWithdrawalPath = configuration["BaseUrl"].ToString() + endPoint;
            depositProductId = DbQueries.GetDepositProductId(dbClientDeposit, depositProductName);
            clientId = DbQueries.GetClientId(dbClient, userEmail);
            accountNumber = DbQueries.GetAccountNumber(dbClientDeposit, depositProductId, clientId);
            DbQueries.CloseDeposit(dbClientDeposit, accountNumber);
            cardNumber = DbQueries.GetCardNumber(dbClientCard, userName, clientId);
            dbController.Connections.TryAdd("1", dbClient);
            cardNumber = DbQueries.GetCardNumber(dbClientCard, userName, clientId);
            DbQueries.DeleteTransactions(dbClientTransactionHistory, accountNumber);
        }

        [Test, Order(1)]
        [AllureStory("Full withdrawal test with valid data")]
        [AllureStep("Full withdrawal test with valid data")]
        public void UserFullWithdrawalValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userMobilePhone,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var depositBalanceQuery = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
            var mapper = MyMapper.Balance.CreateMapper();
            var dbMapper = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQuery);
            var depositBalanceDbBeforeOperation = dbMapper.CurrentBalance;

            var cardBalanceBeforeOperation = apiMethods.GetCardBalance(cardNumber, token);

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber
            };

            var responseFromMakingWithdrawal = apiMethods.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(200);

            if (HooksMethods.KeepTrying(() => HooksMethods.Operation(dbClientDeposit, depositProductId, clientId)))
                depositBalanceDbAfterOperation = 0;
            else
            {
                var depositBalanceQueryAfterOperation = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
                var mapperHelperAfterOperation = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQueryAfterOperation);
                depositBalanceDbAfterOperation = mapperHelperAfterOperation.CurrentBalance;
            }

            depositBalanceDbAfterOperation.Should().Be(0);

            var transactionHistory = DbQueries.CheckTransactions(dbClientTransactionHistory, accountNumber);
            var transactionsMapper = MyMapper.Transactions.CreateMapper();
            var dbMapperForTransaction = transactionsMapper.Map<DataRow, DbHelperTransactions>(transactionHistory);
            dbMapperForTransaction.Amount.Should().Be(depositBalanceDbBeforeOperation);
            dbMapperForTransaction.AccountNumber.Should().Be(accountNumber);
            DbQueries.DeleteTransactions(dbClientTransactionHistory, accountNumber);

            var cardBalanceNew = apiMethods.GetCardBalance(cardNumber, token);
            var differenceInBalanceCard = double.Parse(cardBalanceNew) - double.Parse(cardBalanceBeforeOperation);
            differenceInBalanceCard.Should().Be(depositBalanceDbBeforeOperation);

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = differenceInBalanceCard
            };
           
            var returnToDeposit = apiMethods.GetResponseMakeReplenishDeposit(returnMoney, token);
        }

        [Test, Order(2)]
        [AllureStory("Full withdrawal test with wrong account number")]
        [AllureStep("Full withdrawal test with wrong account number")]
        public void UserFullWithrawalWrongAccountNumberTest()
        {
            var user = new LoginUser
            {
                MobilePhone = userMobilePhone,
                PasswordEncode = userPassword
            };

            ApiMethods usersWork = new ApiMethods();
            var token = usersWork.GetAccessToken(user);

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = string.Empty.CreateRandomString(14),
                CardNumber = cardNumber
            };

            var responseFromMakingWithdrawal = usersWork.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(3)]
        [AllureStory("Full withdrawal test with wrong card number")]
        [AllureStep("Full withdrawal test with wrong card number")]
        public void UserFullWithrawalWrongCardNumberTest()
        {
            var user = new LoginUser
            {
                MobilePhone = userMobilePhone,
                PasswordEncode = userPassword
            };

            ApiMethods usersWork = new ApiMethods();
            var token = usersWork.GetAccessToken(user);

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = accountNumber,
                CardNumber = string.Empty.CreateRandomString(16)
            };

            var responseFromMakingWithdrawal = usersWork.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(4)]
        [AllureStory("Full withdrawal test with empty account number")]
        [AllureStep("Full withdrawal test with empty account number")]
        public void UserFullWithrawalWrongEmptyAccountNumberTest()
        {
            var user = new LoginUser
            {
                MobilePhone = userMobilePhone,
                PasswordEncode = userPassword
            };

            ApiMethods usersWork = new ApiMethods();
            var token = usersWork.GetAccessToken(user);

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = string.Empty,
                CardNumber = cardNumber
            };

            var responseFromMakingWithdrawal = usersWork.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(5)]
        [AllureStory("Full withdrawal test with empty card number")]
        [AllureStep("Full withdrawal test with empty card number")]
        public void UserFullWithrawalWrongEmptyCardNumberTest()
        {
            var user = new LoginUser
            {
                MobilePhone = userMobilePhone,
                PasswordEncode = userPassword
            };

            ApiMethods usersWork = new ApiMethods();
            var token = usersWork.GetAccessToken(user);
            var cardBalance = usersWork.GetCardBalance(cardNumber, token);

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = accountNumber,
                CardNumber = string.Empty
            };

            var responseFromMakingWithdrawal = usersWork.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(6)]
        [AllureStory("Full withdrawal test without authorization")]
        [AllureStep("Full withdrawal test without authorization")]
        public void UserFullWithrawalWithoutAuthorizationTest()
        {

            configuration = Configuration.GetConfiguration();
            var path = configuration["BaseUrl"].ToString() + endPoint;
            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber
            };

            var headers = RequestCreator.CreateHeaders(HeaderTypes.JSON);
            var request = RequestCreator.CreateRequest(fullWithrawalEnqure, headers, path, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessageAsync(request).Result;
            response.StatusCode.CheckStatusCode(401);
        }

        [Test, Order(7)]
        [AllureStory("Full withdrawal test with others token")]
        [AllureStep("Full withdrawal test with others token")]
        public void UserFullWithrawalOthersTokenTest()
        {
            ApiMethods usersWork = new ApiMethods();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRpZCI6ImU4OTIzNTE0LWI1OWMtMTFlYy1iOTA5LTAyNDJhYzEyMDAwMiIsIm5iZiI6MTY3NzU4NjQ0MywiZXhwIjoxNjc3NTg3MzQzLCJpYXQiOjE2Nzc1ODY0NDN9.4G0pnS6d8iC0RNFTUqA0dv_xeHHRVJ01FxoN1iKih3U";

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = accountNumber,
                CardNumber = string.Empty
            };

            var responseFromMakingWithdrawal = usersWork.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(401);
        }

        [Test, Order(8)]
        [AllureStory("Full withdrawal with empty balance data")]
        [AllureStep("Full withdrawal with empty balance data")]
        public void UserFullWithdrawalWithEmptyBalanceTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = "79990234561",
                PasswordEncode = "aqwQQport@43"
            };

            var token = apiMethods.GetAccessToken(user);

            var depositBalanceQuery = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
            var mapper = MyMapper.Balance.CreateMapper();
            var dbMapper = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQuery);
            var depositBalanceDbBeforeOperation = dbMapper.CurrentBalance;

            var cardBalanceBeforeOperation = apiMethods.GetCardBalance(cardNumber, token);

            var fullWithrawalEnqure = new FullWithdrawalModel
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber
            };

            var responseFromMakingWithdrawal = apiMethods.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawal.StatusCode.CheckStatusCode(200);

            var responseFromMakingWithdrawalWithEmptyBalance = apiMethods.GetResponseMakeFullWithdrawal(fullWithrawalEnqure, fullWithdrawalPath, token);
            responseFromMakingWithdrawalWithEmptyBalance.StatusCode.Should().Be(HttpStatusCode.BadRequest, ErrorMessages.BADREQUEST);


            DbQueries.DeleteTransactions(dbClientTransactionHistory, accountNumber);

            var cardBalanceNew = apiMethods.GetCardBalance(cardNumber, token);
            var differenceInBalanceCard = double.Parse(cardBalanceNew) - double.Parse(cardBalanceBeforeOperation);
            differenceInBalanceCard.Should().Be(depositBalanceDbBeforeOperation);

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = differenceInBalanceCard
            };

            var returnToDeposit = apiMethods.GetResponseMakeReplenishDeposit(returnMoney, token);
        }

        [TearDown]
        public void TearDown()
        {
            dbClient.Dispose();
            dbClientDeposit.Dispose();
            dbClientCard.Dispose();
            service.Dispose();
            dbController.Connections.Clear();
        }
    }
}
