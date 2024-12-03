using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_DbTestingCore.DbControllers;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Extensions;
using System.Net;

namespace PowerBank_AQA_ApiTesting.Tests.PaymentService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-5.2 Совершить перевод по номеру карты.")]
    [AllureFeature("EP-5.2 Совершить перевод по номеру карты.")]
    [AllureEpic("Info service")]
    public class EP5_2_PostTransferByCardNumber
    {
        private WebService service;
        private IConfiguration configuration;
        private readonly string senderCardNumber = "4123452108579126";
        private readonly string cardVerification = "789";
        private readonly string recipientCardNumber = "5123459671906923";
        private readonly string recipientEmptyCardNumber = "";
        private readonly string senderAnotherClientCardNumber = "5123453724924666";
        private readonly string anotherClientCardVerification = "345";
        private readonly string сurrencyRUBCode = "RUB";
        private readonly string сurrencyUSDCode = "USD";
        private readonly decimal amountTransfer = 10.00M;
        private readonly decimal amountNotEnoughTransfer = 0.01M;
        private readonly decimal amountCommission = 0.00M;
        private readonly decimal amountWithdrawal = 10.00M;
        private readonly decimal amountNotEnoughWithdrawal = 0.01M;
        private readonly string transactionComment = "на подарок";
        private readonly string userNumber = "79772345685";
        private readonly string userPassword = "ls23Ghq#wEr";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
        }

        [Test, Order(1)]
        [AllureStory("Совершить перевод по номеру карты (валидные данные)")]
        [AllureStep("Совершить перевод по номеру карты (валидные данные)")]
        public void TransferByCardNumberValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal,
                TransactionComment = transactionComment
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(2)]
        [AllureStory("Совершить перевод по номеру карты (без указания карты получателя)")]
        [AllureStep("Совершить перевод по номеру карты (без указания карты получателя)")]
        public void TransferByCardNumberWithoutRecipientCardTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);
            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientEmptyCardNumber,
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(3)]
        [AllureStory("Совершить перевод по номеру карты (без получения accessToken)")]
        [AllureStep("Cовершить перевод по номеру карты (без получения accessToken)")]
        public void TransferByCardNumberWithoutAuthorizationTest()
        {
            ApiMethods apiMethods = new ApiMethods();

            var token = string.Empty;
            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(401);
        }


        [Test, Order(4)]
        [AllureStory("Cовершить перевод по номеру карты (Перевод с карты другого клиента банка)")]
        [AllureStep("Cовершить перевод по номеру карты (Перевод с карты другого клиента банка)")]
        public void TransferByCardSenderCardIsOtherBankTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderAnotherClientCardNumber,
                CardVerification = anotherClientCardVerification,
                RecipientCardNumber = recipientCardNumber,
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(404);
        }


        [Test, Order(5)]
        [AllureStory("Cовершить перевод по номеру карты (без указания валюты)")]
        [AllureStep("Cовершить перевод по номеру карты (без указания валюты)")]
        public void TransferByCardWithoutCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                TransactionComment = transactionComment,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }


        [Test, Order(6)]
        [AllureStory("Cовершить перевод по номеру карты (в валюте отличной от валюты счёта)")]
        [AllureStep("Cовершить перевод по номеру карты (в валюте отличной от валюты счёта)")]
        public void TransferByCardWrongCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyUSDCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }


        [Test, Order(7)]
        [AllureStory("Cовершить перевод по номеру карты (сумма перевода = 0)")]
        [AllureStep("Cовершить перевод по номеру карты (сумма перевода = 0)")]
        public void TransferByCardSumEqualZeroTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = 0,
                AmountCommission = 0,
                AmountWithdrawal = 0,
                TransactionComment = transactionComment
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }


        [Test, Order(8)]
        [AllureStory("Cовершить перевод по номеру карты (номер карты получателя = 15 цифр)")]
        [AllureStep("Cовершить перевод по номеру карты (номер карты получателя = 15 цифр)")]
        public void TransferByCardRecipientCardContains15NumbersTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = string.Empty.CreateRandomStringNumbers(15),
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(9)]
        [AllureStory("Cовершить перевод по невалидному номеру карты (номер карты получателя - 16 латинских букв)")]
        [AllureStep("Cовершить перевод по невалидному номеру карты (номер карты получателя - 16 латинских букв)")]
        public void TransferByNotValidCardRecipientCardContains16LettersTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = string.Empty.CreateRandomString(16),
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }


        [Test, Order(10)]
        [AllureStory("Cовершить перевод по номеру карты (сумма перевода меньше 1.00 RUR)")]
        [AllureStep("Cовершить перевод по номеру карты (сумма перевода меньше 1.00 RUR)")]
        public void TransferByCardSumTransactionLessThanOneRubleTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                TransactionComment = transactionComment,
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountNotEnoughTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountNotEnoughWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(11)]
        [AllureStep("Значения поля комментария")]
        public void TransferByCardWithNotValidComment()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                SenderCardNumber = senderCardNumber,
                CardVerification = cardVerification,
                RecipientCardNumber = recipientCardNumber,
                TransactionComment = string.Empty.CreateRandomString(10),
                CurrencyCode = сurrencyRUBCode,
                AmountTransfer = amountTransfer,
                AmountCommission = amountCommission,
                AmountWithdrawal = amountWithdrawal
            };
            var response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(200);

            possibilityCardTransactions.TransactionComment = string.Empty.CreateRandomString(8) + string.Empty.CreateRandomStringSpecialCharacters(2);
            response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(200);

            possibilityCardTransactions.TransactionComment = string.Empty.CreateRandomString(51);
            response = apiMethods.CheckTransferBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }
    }
}
