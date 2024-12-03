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

namespace PowerBank_AQA_ApiTesting.Tests.CardService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-4.6 Проверка возможности совершить перевод по номеру карты")]
    [AllureFeature("EP-4.6 Проверка возможности совершить перевод по номеру карты")]
    [AllureEpic("Card service")]
    public class EP4_6_PossibiltyTransactionBetweenCardsTest
    {
        private WebService service;
        private IConfiguration configuration;
        private readonly string senderCardNumber = "2123451947067742";
        private readonly string recipientCardNumber = "2123454589539179";
        private DbClient dbClientCard;
        private readonly string userNumber = "76666666666";
        private readonly string userPassword = "Ihave6Cards!";
  
        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
        }

        [Test, Order(1)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты")]
        public void PossibiltyTransactionBetweenCardsValidDataTest()
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
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = senderCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(2)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты с просроченным токеном")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты с просроченным токеном")]
        public void PossibiltyTransactionBetweenCardsWithWrongTokenTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRpZCI6ImU4OTIzNTE0LWI1OWMtMTFlYy1iOTA5LTAyNDJhYzEyMDAwMiIsIm5iZiI6MTY3NzU4NjQ0MywiZXhwIjoxNjc3NTg3MzQzLCJpYXQiOjE2Nzc1ODY0NDN9.4G0pnS6d8iC0RNFTUqA0dv_xeHHRVJ01FxoN1iKih3U";

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = senderCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(401);
        }

        [Test, Order(3)]
        [AllureStory("Проверка возможности совершить перевод между картами с одинаковыми номерами")]
        [AllureStep("Проверка возможности совершить перевод между картами с одинаковыми номерами")]
        public void PossibiltyTransactionBetweenCardsWithSameNumberTest()
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
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = recipientCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("errors");
            responseMessage.Should().Contain($"Значения не должны совпадать", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(4)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты на заблокированную карту")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты на заблокированную карту")]
        public void PossibiltyTransactionBetweenCardsToBlockCardTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = "76666666666",
                PasswordEncode = "Ihave6Cards!"
            };

            dbClientCard = new DbClient(configuration, "CardDb");
            dbClientCard.Create();
            var cardForBlock = "5123451104453485";
            DbQueries.BlockCard(dbClientCard, cardForBlock);
            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = "5123451268390297",
                SenderCardNumber = cardForBlock
            };

            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"Card {cardForBlock} is unavailable", ErrorMessages.INCORRECTRESPONSEMESSAGE);
            DbQueries.UnblockCard(dbClientCard, cardForBlock);
            dbClientCard.Dispose();
        }

        [Test, Order(5)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты с заблокированной карты")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты с заблокированной")]
        public void PossibiltyTransactionBetweenCardsFromBlockCardTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = "76666666666",
                PasswordEncode = "Ihave6Cards!"
            };

            dbClientCard = new DbClient(configuration, "CardDb");
            dbClientCard.Create();
            var cardForBlock = "5123451104453485";
            DbQueries.BlockCard(dbClientCard, cardForBlock);
            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = cardForBlock,
                SenderCardNumber = "5123451268390297"
            };

            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"Recipient card number is not valid or blocked.", ErrorMessages.INCORRECTRESPONSEMESSAGE);
            DbQueries.UnblockCard(dbClientCard, cardForBlock);
            dbClientCard.Dispose();
        }

        [Test, Order(6)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты с просроченной карты")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты с просроченной карты")]
        public void PossibiltyTransactionBetweenCardsFromWrongExpirationDateCardTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = "79772345685",
                PasswordEncode = "ls23Ghq#wEr"
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = "5123459569776203",
                SenderCardNumber = "4123459784318944"
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"Card {possibilityCardTransactions.SenderCardNumber} is unavailable", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(7)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты на просроченную карту")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты на просроченную карту")]
        public void PossibiltyTransactionBetweenCardsToWrongExpirationDateCardTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = "79772345685",
                PasswordEncode = "ls23Ghq#wEr"
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = "5123459569776203",
                SenderCardNumber = "4123459784318944"
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
            var responseMessage = response.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"Card {possibilityCardTransactions.SenderCardNumber} is unavailable", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(8)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты с одинаковым счетом")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты с одинаковым счетом")]
        public void PossibiltyTransactionBetweenCardsWithCommonAccountTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = "76666666666",
                PasswordEncode = "Ihave6Cards!"
            };

            var token = apiMethods.GetAccessToken(user);

            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = "5123451104453485",
                SenderCardNumber = "2123451947067742"
            };

            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400); 
            var responseMessage = response.ResponseBody.GetJsonValueByKey("Message");
            responseMessage.Should().Contain($"Cards depend to one account.", ErrorMessages.INCORRECTRESPONSEMESSAGE);
        }

        [Test, Order(9)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (неверный номер отправителя)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (неверный номер отправителя)")]
        public void PossibiltyTransactionBetweenCardsWithWrongSenderCardNumberTest()
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
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = string.Empty.CreateRandomStringNumbers(16)
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(404);
        }

        [Test, Order(10)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты с чужим токеном")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты с чужим токеном")]
        public void PossibiltyTransactionBetweenCardsWithAnothersTokenTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRpZCI6ImU4OTIzNTE0LWI1OWMtMTFlYy1iOTA5LTAyNDJhYzEyMDAwMiIsIm5iZiI6MTY5MTQxOTYzMSwiZXhwIjoxNjkxNDIwNTMxLCJpYXQiOjE2OTE0MTk2MzF9.CotHDEKRc0Bp19L4-uAT0JhQF7G7VVjopyFIgZegftM";
            var possibilityCardTransactions = new PossibilityCardTransactions
            {
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = senderCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(401);
        }

        [Test, Order(11)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (карта-отправитель содержит более 16 цифр)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (карта-отправитель содержит более 16 цифр)")]
        public void PossibiltyTransactionBetweenCardsSenderCardNumberMoreThan16Test()
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
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = string.Empty.CreateRandomStringNumbers(20)
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(12)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (карта-отправитель содержит спецсимволы и буквы)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (карта-отправитель содержит спецсимволы и буквы)")]
        public void PossibiltyTransactionBetweenCardsSenderCardNumberWithSpecialSymbolsAndLettersTest()
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
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = string.Empty.CreateRandomStringNumbers(12) + string.Empty.CreateRandomStringRusLetters(2) + string.Empty.CreateRandomStringSpecialCharacters(2)
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(13)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (карта-отправитель содержит пустое значение)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (карта-отправитель содержит пустое значение)")]
        public void PossibiltyTransactionBetweenCardsSenderCardNullTest()
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
                RecipientCardNumber = recipientCardNumber,
                SenderCardNumber = ""
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(14)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (карта-получатель содержит более 16 цифр)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (карта-получатель содержит более 16 цифр)")]
        public void PossibiltyTransactionBetweenCardsRecipientCardNumberMoreThan16Test()
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
                RecipientCardNumber = string.Empty.CreateRandomStringNumbers(20),
                SenderCardNumber = senderCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(15)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (карта-получатель содержит спецсимволы и буквы)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (карта-получатель содержит спецсимволы и буквы)")]
        public void PossibiltyTransactionBetweenCardsRecipientCardNumberWithSpecialSymbolsAndLettersTest()
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
                RecipientCardNumber = string.Empty.CreateRandomStringNumbers(12) + string.Empty.CreateRandomStringRusLetters(2) + string.Empty.CreateRandomStringSpecialCharacters(2),
                SenderCardNumber = senderCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(16)]
        [AllureStory("Проверка возможности совершить перевод по номеру карты (карта-получатель содержит пустое значение)")]
        [AllureStep("Проверка возможности совершить перевод по номеру карты (карта-получатель содержит пустое значение)")]
        public void PossibiltyTransactionBetweenCardsRecipientCardNullTest()
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
                RecipientCardNumber = "",
                SenderCardNumber = senderCardNumber
            };
            var response = apiMethods.CheckPossibilityTransactionBetweenCard(token, possibilityCardTransactions);
            response.StatusCode.CheckStatusCode(400);
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }
    }
}
