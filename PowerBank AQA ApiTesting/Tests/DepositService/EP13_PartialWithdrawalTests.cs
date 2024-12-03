using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.ApiRequest;
using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_ApiTestingCore.Models.Dto;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Extensions;
using System.Data;

namespace PowerBank_AQA_ApiTesting.Tests.DepositService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-3.13 Частичное снятие средств с депозита ")]
    [AllureFeature("EP-3.13 Частичное снятие средств с депозита ")]
    [AllureEpic("Deposit service")]
    public class EP13_PartialWithdrawalTests
    {
        private WebService service;
        private IConfiguration configuration;
        private DbController dbController;
        private DbClient dbClient;
        private DbClient dbClientDeposit;
        private DbClient dbClientCard;
        private DbClient dbClientTransaction;
        private string depositProductId;
        private string clientId;
        private string accountNumber;
        private string cardNumber;
        private int minAmountSumOnDeposit;
        private int minSumForTransaction = 1;
        private double depositBalanceDbAfterOperation;
        private readonly string depositProductName = "SummerOffer";
        private readonly string userEmail = "korneychyk20@fake.ru";
        private readonly string userName = "Semyon Korneichuk";
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
            dbClientTransaction = new DbClient(configuration, "TransactionHistoryDb");
            dbClientTransaction.Create();
            depositProductId = DbQueries.GetDepositProductId(dbClientDeposit, depositProductName);
            clientId = DbQueries.GetClientId(dbClient, userEmail);
            accountNumber = DbQueries.GetAccountNumber(dbClientDeposit, depositProductId, clientId);
            DbQueries.DeleteTransactions(dbClientTransaction, accountNumber);
            DbQueries.CloseDeposit(dbClientDeposit, accountNumber);
            cardNumber = DbQueries.GetCardNumber(dbClientCard, userName, clientId);
            minAmountSumOnDeposit = DbQueries.CheckAmountMinTransaction(dbClientDeposit, depositProductName);
            dbController.Connections.TryAdd("1", dbClient);
        }

        [Test, Order(1)]
        [AllureStory("Partial withdrawal test with valid data")]
        [AllureStep("Partial withdrawal test with valid data")]
        public void PartialWithdrawalValidDataTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = 2000
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(200);

            if (HooksMethods.KeepTrying(() => HooksMethods.Operation(dbClientDeposit, depositProductId, clientId)))
                depositBalanceDbAfterOperation = 0;
            else
            {
                var depositBalanceQueryAfterOperation = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
                var mapperHelperAfterOperation = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQueryAfterOperation);
                depositBalanceDbAfterOperation = mapperHelperAfterOperation.CurrentBalance;
            }

            var differenceBalanceDb = depositBalanceDbBeforeOperation - depositBalanceDbAfterOperation;
            differenceBalanceDb.Should().Be(returnMoney.EnteredAmount);

            var cardBalanceAfterOperation = apiMethods.GetCardBalance(cardNumber, token);
            var differenceBalanceCard = double.Parse(cardBalanceAfterOperation) - double.Parse(cardBalanceBeforeOperation);
            differenceBalanceCard.Should().Be(returnMoney.EnteredAmount);

            var returnToDeposit = apiMethods.GetResponseMakeReplenishDeposit(returnMoney, token);
        }

        [Test, Order(2)]
        [AllureStory("Partial withdrawal test with minAmount transaction")]
        [AllureStep("Partial withdrawal test with minAmount transaction")]
        public void PartialWithdrawalMinAmountTransactionTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = minSumForTransaction
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(200);

            if (HooksMethods.KeepTrying(() => HooksMethods.Operation(dbClientDeposit, depositProductId, clientId)))
                depositBalanceDbAfterOperation = 0;
            else
            {
                var depositBalanceQueryAfterOperation = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
                var mapperHelperAfterOperation = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQueryAfterOperation);
                depositBalanceDbAfterOperation = mapperHelperAfterOperation.CurrentBalance;
            }

            var differenceBalanceDb = depositBalanceDbBeforeOperation - depositBalanceDbAfterOperation;
            differenceBalanceDb.Should().Be(returnMoney.EnteredAmount);

            var cardBalanceAfterOperation = apiMethods.GetCardBalance(cardNumber, token);
            var differenceBalanceCard = double.Parse(cardBalanceAfterOperation) - double.Parse(cardBalanceBeforeOperation);
            differenceBalanceCard.Should().Be(returnMoney.EnteredAmount);

            var returnToDeposit = apiMethods.GetResponseMakeReplenishDeposit(returnMoney, token);
            returnToDeposit.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(3)]
        [AllureStory("Partial withdrawal test with franctionalSum transaction")]
        [AllureStep("Partial withdrawal test with franctionalSum transaction")]
        public void PartialWithdrawalFranctionalNumberTransactionTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = 400.5
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(200);

            if (HooksMethods.KeepTrying(() => HooksMethods.Operation(dbClientDeposit, depositProductId, clientId)))
                depositBalanceDbAfterOperation = 0;
            else
            {
                var depositBalanceQueryAfterOperation = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
                var mapperHelperAfterOperation = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQueryAfterOperation);
                depositBalanceDbAfterOperation = mapperHelperAfterOperation.CurrentBalance;
            }

            var differenceBalanceDb = depositBalanceDbBeforeOperation - depositBalanceDbAfterOperation;
            differenceBalanceDb.Should().Be(returnMoney.EnteredAmount);

            var cardBalanceAfterOperation = apiMethods.GetCardBalance(cardNumber, token);
            var differenceBalanceCard = double.Parse(cardBalanceAfterOperation) - double.Parse(cardBalanceBeforeOperation);
            differenceBalanceCard.Should().Be(returnMoney.EnteredAmount);

            var returnToDeposit = apiMethods.GetResponseMakeReplenishDeposit(returnMoney, token);
            returnToDeposit.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(4)]
        [AllureStory("Partial withdrawal test left minAmount on deposit")]
        [AllureStep("Partial withdrawal test left minAmount on deposit")]
        public void PartialWithdrawalLeftMinAmountOnDepositTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = depositBalanceDbBeforeOperation - minAmountSumOnDeposit
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(200);

            if (HooksMethods.KeepTrying(() => HooksMethods.Operation(dbClientDeposit, depositProductId, clientId)))
                depositBalanceDbAfterOperation = 0;
            else
            {
                var depositBalanceQueryAfterOperation = DbQueries.CheckAccountNumberAndDepositBalance(dbClientDeposit, depositProductId, clientId);
                var mapperHelperAfterOperation = mapper.Map<DataRow, DbHelperBalance>(depositBalanceQueryAfterOperation);
                depositBalanceDbAfterOperation = mapperHelperAfterOperation.CurrentBalance;
            }

            var differenceBalanceDb = depositBalanceDbBeforeOperation - depositBalanceDbAfterOperation;
            differenceBalanceDb.Should().Be(returnMoney.EnteredAmount);

            var cardBalanceAfterOperation = apiMethods.GetCardBalance(cardNumber, token);
            var differenceBalanceCard = double.Parse(cardBalanceAfterOperation) - double.Parse(cardBalanceBeforeOperation);
            differenceBalanceCard.Should().Be(returnMoney.EnteredAmount);

            var returnToDeposit = apiMethods.GetResponseMakeReplenishDeposit(returnMoney, token);
            returnToDeposit.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(5)]
        [AllureStory("Partial withdrawal test with wrong account")]
        [AllureStep("Partial withdrawal test with wrong account")]
        public void PartialWithdrawalWrongAccountTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = string.Empty.CreateRandomStringNumbers(14),
                CardNumber = cardNumber,
                EnteredAmount = 2000
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(404);
            var responseMessage = partialWithdrawalResponse.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"The requested Account was not found with searched parameters :AccountNumber:{returnMoney.AccountNumber}", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(6)]
        [AllureStory("Partial withdrawal test with 0 EnteredAmount")]
        [AllureStep("Partial withdrawal test with 0 EnteredAmount")]
        public void PartialWithdrawalWithZeroEnteredAmountTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = 0
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
            var responseMessage = partialWithdrawalResponse.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"'Entered Amount' must be greater than or equal to '1'", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(7)]
        [AllureStory("Partial withdrawal test with EnteredAmount less 1")]
        [AllureStep("Partial withdrawal test with EnteredAmount less 1")]
        public void PartialWithdrawalWithEnteredAmountLessOneTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = 0.99
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
            var responseMessage = partialWithdrawalResponse.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"'Entered Amount' must be greater than or equal to '1'", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(8)]
        [AllureStory("Partial withdrawal left deposit amount less than required test")]
        [AllureStep("Partial withdrawal left deposit amount less than required test")]
        public void PartialWithdrawalLeftDepositAmountLessThanRequiredTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = depositBalanceDbBeforeOperation - minAmountSumOnDeposit + 1
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
            var responseMessage = partialWithdrawalResponse.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"Account with number {accountNumber} is using deposit product SummerOffer. Withdrawal amount your entered is not permitted. Min balance shouldn't be less than 1000.00, due deposit product conditions", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(9)]
        [AllureStory("Partial withdrawal with fractional amount with 3 digits after dot test")]
        [AllureStep("Partial withdrawal with fractional amount with 3 digits after dot test")]
        public void PartialWithdrawalWithFractionalAmountWithThreeDigitsAfterDotTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = 2000.555
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
            var responseMessage = partialWithdrawalResponse.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Entered Amount' must not be more than 20 digits in total, with allowance for 2 decimals", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(10)]
        [AllureStory("Partial withdrawal without AccountNumber test")]
        [AllureStep("Partial withdrawal without AccountNumber test")]
        public void PartialWithdrawalWithoutAccountNumberTest()
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

            var returnMoney = new DepositCardTransations
            {
                CardNumber = cardNumber,
                EnteredAmount = minAmountSumOnDeposit
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(11)]
        [AllureStory("Partial withdrawal without CardNumber test")]
        [AllureStep("Partial withdrawal without CardNumber test")]
        public void PartialWithdrawalWithoutCardNumberTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                EnteredAmount = minAmountSumOnDeposit
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(12)]
        [AllureStory("Partial withdrawal without EnteredAmount test")]
        [AllureStep("Partial withdrawal without EnteredAmount test")]
        public void PartialWithdrawalWithoutEnteredAmountTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(13)]
        [AllureStory("Partial withdrawal with inappropriate AccountNumber test")]
        [AllureStep("Partial withdrawal with inappropriate AccountNumber test")]
        public void PartialWithdrawalWithInappropriateAccountNumberTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = "true",
                CardNumber = cardNumber,
                EnteredAmount = 2000
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(14)]
        [AllureStory("Partial withdrawal with inappropriate CardNumber test")]
        [AllureStep("Partial withdrawal with inappropriate CardNumber test")]
        public void PartialWithdrawalWithInappropriateCardNumberTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = "true",
                EnteredAmount = 2000
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(15)]
        [AllureStory("Partial withdrawal with inappropriate EnteredAmount test")]
        [AllureStep("Partial withdrawal with inappropriate EnteredAmount test")]
        public void PartialWithdrawalWithInappropriateEnteredAmountTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = -9000
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(16)]
        [AllureStory("Partial withdrawal with only AccountNumber test")]
        [AllureStep("Partial withdrawal with only AccountNumber  test")]
        public void PartialWithdrawalWithOnlyAccountNumberTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(17)]
        [AllureStory("Partial withdrawal with only CardNumber test")]
        [AllureStep("Partial withdrawal with only CardNumbertest")]
        public void PartialWithdrawalWithOnlyCardNumberTest()
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

            var returnMoney = new DepositCardTransations
            {
                CardNumber = cardNumber
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(18)]
        [AllureStory("Partial withdrawal with only EnteredAmount test")]
        [AllureStep("Partial withdrawal with only EnteredAmount test")]
        public void PartialWithdrawalWithOnlyEnteredAmountTest()
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

            var returnMoney = new DepositCardTransations
            {
                EnteredAmount = 2000
            };

            var partialWithdrawalResponse = apiMethods.GetResponseParticialWithdrawalFromDeposit(returnMoney, token);
            partialWithdrawalResponse.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(19)]
        [AllureStory("Partial withdrawal without authorization test")]
        [AllureStep("Partial withdrawal without authorization test")]
        public void PartialWithdrawalWithoutAuthorizationTest()
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

            var returnMoney = new DepositCardTransations
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                EnteredAmount = 2000
            };

            var endPoint = configuration["BaseUrl"].ToString() + "api/v1/deposit/accounts/partwithdrawal";

            var headers = RequestCreator.CreateHeaders(HeaderTypes.JSON);
            var request = RequestCreator.CreateRequest(returnMoney, headers, endPoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessageAsync(request).Result;
            response.StatusCode.CheckStatusCode(401);
        }

        [TearDown]
        public void TearDown()
        {
            dbClient.Dispose();
            dbClientDeposit.Dispose();
            dbClientTransaction.Dispose();
            dbClientCard.Dispose();
            service.Dispose();
            dbController.Connections.Clear();
        }
    }
}
