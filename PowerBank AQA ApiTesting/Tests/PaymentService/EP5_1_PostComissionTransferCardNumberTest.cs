using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Schema;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTesting.Hooks;
using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_TestingCore.Configuration;
using PowerBank_AQA_TestingCore.Extensions;

namespace PowerBank_AQA_ApiTesting.Tests.PaymentService
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("EP-5.1 Получить размер комиссии за перевод")]
    [AllureFeature("EP-5.1 Получить размер комиссии за перевод")]
    [AllureEpic("Info service")]
    public class EP5_1_PostComissionTransferCardNumberTest
    {
        private WebService service;
        private IConfiguration configuration;
        private readonly string userNumber = "79772345685";
        private readonly string userPassword = "ls23Ghq#wEr";
        private readonly string currencyCodeValid = "RUB";
        private readonly string recipientBinValid = "512345";

        [SetUp]
        public void Setup()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
        }

        [Test, Order(1)]
        [AllureStory("Получить размер комиссии за перевод")]
        [AllureStep("Получить размер комиссии за перевод")]
        public void CheckCommissionTransferByCardNumberValidDataTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(200);
        }

        [Test, Order(2)]
        [AllureStory("Получить размер комиссии за перевод неавторизованным пользователем")]
        [AllureStep("Получить размер комиссии за перевод неавторизованным пользователем")]
        public void CheckCommissionTransferByCardNumberWithoutAuthorizationTest()
        {
            ApiMethods apiMethods = new ApiMethods();

            var token = string.Empty;

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(401);
        }

        [Test, Order(3)]
        [AllureStory("Получить размер комиссии за перевод невалидными данными BIN (символы вместо цифры)")]
        [AllureStep("Получить размер комиссии за перевод невалидными данными BIN (символы вместо цифры)")]
        public void CheckCommissionTransferByCardNumberSymbolsInBINTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = string.Empty.CreateRandomStringSpecialCharacters(6),
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(4)]
        [AllureStory("Получить размер комиссии за перевод невалидными данными BIN (пропись буквами)")]
        [AllureStep("Получить размер комиссии за перевод невалидными данными BIN (пропись буквами)")]
        public void CheckCommissionTransferByCardNumberLettersInBINTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = string.Empty.CreateRandomStringRusLetters(6)
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(5)]
        [AllureStory("Получить размер комиссии за перевод невалидными данными BIN (пропись латиницей)")]
        [AllureStep("Получить размер комиссии за перевод невалидными данными BIN (пропись латиницей)")]
        public void CheckCommissionTransferByCardNumberLatinInBINTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = string.Empty.CreateRandomString(6)
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(6)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (код валюты прописан не латиницей)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (код валюты прописан не латиницей)")]
        public void CheckCommissionTransferByCardNumberNotLatinCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = "руб",
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(7)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (код валюты прописан не заглавными латинскими буквами)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (код валюты прописан не заглавными латинскими буквами)")]
        public void CheckCommissionTransferByCardNumberNotTitleLettersCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = "rub",
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(8)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (в коде валюты не все буквы заглавные)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (в коде валюты не все буквы заглавные)")]
        public void CheckCommissionTransferByCardNumberNotAllLettersAreTitleCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = "RUb",
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(9)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (несуществующая валюта)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты с невалидным CurrencyCode (несуществующая валюта)")]
        public void CheckCommissionTransferByCardNumberNotExistingCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = "ABC",
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(10)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты без обязательных параметров")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты без обязательных параметров")]
        public void CheckCommissionTransferByCardNumberWithoutRequiredParametersTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = string.Empty,
                RecipientBin = string.Empty
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(11)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты без ввода CurrencyCode")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты без ввода CurrencyCode")]
        public void CheckCommissionTransferByCardNumberWithoutCurrencyCodeTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = string.Empty,
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(12)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты (Bin менее 6 цифр)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты (Bin менее 6 цифр)")]
        public void CheckCommissionTransferByCardNumberBinLessThanSixIntegersTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = string.Empty.CreateRandomStringNumbers(5)
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(13)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты (Bin более 6 цифр)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты (Bin более 6 цифр)")]
        public void CheckCommissionByCardNumberBinMoreThanSixIntegersTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = currencyCodeValid,
                RecipientBin = string.Empty.CreateRandomStringNumbers(7)
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(14)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты невалидный CurrencyCode (длина кода валюты более 3 символов)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты невалидный CurrencyCode (длина кода валюты более 3 символов)")]
        public void CheckCommissionByCardNumberCurrencyCodeMoreThanThreeSymbolsTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = "RUBLEY",
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [Test, Order(15)]
        [AllureStory("Получить размер комиссии за перевод по номеру карты с невалидными данными CurrencyCode (длина кода валюты менее 3х символов)")]
        [AllureStep("Получить размер комиссии за перевод по номеру карты с невалидными данными CurrencyCode (длина кода валюты менее 3х символов)")]
        public void CheckCommissionByCardNumberCurrencyCodeLessThanThreeSymbolsTest()
        {
            ApiMethods apiMethods = new ApiMethods();
            var user = new LoginUser
            {
                MobilePhone = userNumber,
                PasswordEncode = userPassword
            };

            var token = apiMethods.GetAccessToken(user);

            var commissionTransferCardNumber = new CommissionTransferCardNumber
            {
                CurrencyCode = "US",
                RecipientBin = recipientBinValid
            };
            var response = apiMethods.GetCommissionTransferCardNumber(token, commissionTransferCardNumber);
            response.StatusCode.CheckStatusCode(400);
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }
    }
}
