using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using PowerBank_AQA_ApiTesting.AllureSteps;
using PowerBank_AQA_ApiTesting.ClassData;
using PowerBank_AQA_ApiTestingCore.ApiRequest;
using PowerBank_AQA_ApiTestingCore.Extensions;
using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_DbTestingCore.DbClient;
using PowerBank_AQA_TestingCore.Configuration;

namespace PowerBank_AQA_ApiTesting.Hooks
{
    public class ApiMethods
    {
        private string loginPathPhone;
        private WebService service;
        private IConfiguration configuration;
        private string endPoint;
        private string cardbalanceFullEndPoint;
        private string registrationPath;
        private LoginUser loginUser;
        private readonly string registrationEndPoint = "api/v1/user/registration/user-profile/new";
        private readonly string endPointPhoneLogin = "api/v1/user/login/mobile_phone";
        private readonly string checkCardBalanceEndPoing = "api/v1/card/agreements/get-card-balance/";
        private readonly string depositReplenishment = "api/v1/deposit/accounts/replenish";
        private readonly string depositPartWithdrawal = "api/v1/deposit/accounts/partwithdrawal";
        private readonly string depositInfo = "api/v1/deposit/accounts";
        private readonly string transactionsBetweenCards = "api/v1/card/transfer/check";
        private readonly string possibilityCardTransfer = "api/v1/payment/transfer/by-card";
        private readonly string commissionTransfer = "api/v1/payment/transfer/commission";
        private readonly string transactionHistoryCard = "api/v1/transaction-history/card";
        private readonly string availableCards = $"api/v1/card/cards";
        private readonly string databaseCardRecreateEndPoint = "api/v1/card/database-recreate";
        private readonly string databaseDepositRecreateEndPoint = "api/v1/deposit/database-recreate";
        private readonly string databaseCreditRecreateEndPoint = "api/v1/credit/database-recreate";
        private readonly string databaseUserRecreateEndPoint = "api/v1/user/database-recreate";
        private readonly string databaseTransactionHistoryRecreateEndPoint = "api/v1/transaction-history/database-recreate";
        private readonly string emailEndPoint = "api/v1/user/settings/email";
        private readonly string notifications = "api/v1/user/settings/notifications/email";



        private void StartService()
        {
            service = new WebService();
            configuration = Configuration.GetConfiguration();
        }

        public ResponseInfo GetResponseFromPostNewUser(RegistrationNewUser newUser)
        {
            StartService();
            registrationPath = configuration["BaseUrl"].ToString() + registrationEndPoint;

            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(newUser, headers, registrationPath, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public string GetAccessToken(LoginUser loginUser)
        {
            StartService();
            loginPathPhone = configuration["BaseUrl"].ToString() + endPointPhoneLogin;

            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(loginUser, headers, loginPathPhone, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            var accessToken = response.ResponseBody.GetValueByKee("accessToken");
            return accessToken;
        }

        public string GetCardBalance(string cardNumber, string token)
        {
            StartService();
            cardbalanceFullEndPoint = configuration["BaseUrl"].ToString() + checkCardBalanceEndPoing + cardNumber;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(cardNumber, headers, cardbalanceFullEndPoint, HttpMethod.Get, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            var cardBalance = response.ResponseBody.GetValueByKee("balance");
            return cardBalance;
        }

        public ResponseInfo GetResponseMakeFullWithdrawal(FullWithdrawalModel fullWithdrawalModel, string withdrawalEndpoint, string token)
        {
            StartService();
            configuration = Configuration.GetConfiguration();

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(fullWithdrawalModel, headers, withdrawalEndpoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo GetResponseMakeReplenishDeposit(DepositCardTransations depositCardTransactions, string token)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + depositReplenishment;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(depositCardTransactions, headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo GetResponseParticialWithdrawalFromDeposit(DepositCardTransations depositCardTransation, string token)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + depositPartWithdrawal;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(depositCardTransation, headers, endPoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo GetResponseCheckDepositBalance(string token, string accountNumber)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + depositInfo +"/"+ accountNumber;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Get, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo CheckPossibilityTransactionBetweenCard(string token, PossibilityCardTransactions possibilityCardTransactions)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + transactionsBetweenCards;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(possibilityCardTransactions, headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo GetCommissionTransferCardNumber(string token, CommissionTransferCardNumber commissionTransferCardNumber)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + commissionTransfer + "?recipientBin=" + commissionTransferCardNumber.RecipientBin + "&currencyCode=" + commissionTransferCardNumber.CurrencyCode + "";

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Get, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo GetAllActiveClientsDeposits(string token)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + depositInfo;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Get, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo CheckTransactionsHistoryCard(string token, CardTransactionHistory cardTransactionHistory)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + transactionHistoryCard;

            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(cardTransactionHistory, headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo LockingCard(string token, string cardNumber)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + availableCards + "/" + cardNumber + "/block";
            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo UnlockingCard(string token, string cardNumber)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + availableCards + "/" + cardNumber + "/unblock";
            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public void RecreatingDatabaseCard()
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + databaseCardRecreateEndPoint;
            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
        }

        public void RecreatingDatabaseDeposit()
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + databaseDepositRecreateEndPoint;
            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
        }

        public void RecreatingDatabaseUsers()
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + databaseUserRecreateEndPoint;
            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
        }

        public void RecreatingDatabaseCredit()
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + databaseCreditRecreateEndPoint;
            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
        }

        public void RecreatingDatabaseTransactionHistory()
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + databaseTransactionHistoryRecreateEndPoint;
            var headers = ApiAllureSteps.CreateHeaders();
            var request = ApiAllureSteps.CreateRequest(headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
        }

        public ResponseInfo ChangeEmail(string token, Email newEmail)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + emailEndPoint;
            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(newEmail, headers, endPoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo CheckTransferBetweenCard(string token, PossibilityCardTransactions possibilityCardTransactions)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + possibilityCardTransfer;
            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(possibilityCardTransactions, headers, endPoint, HttpMethod.Post, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }

        public ResponseInfo ChangeNotification(string token, Notification notification)
        {
            StartService();
            endPoint = configuration["BaseUrl"].ToString() + notifications;
            var headers = ApiAllureSteps.CreateHeaders();
            headers.AddHeader(("Authorization", "Bearer " + token));
            var request = ApiAllureSteps.CreateRequest(notification, headers, endPoint, HttpMethod.Patch, int.Parse(configuration["timeOut"]));
            var response = service.SendMessage(request);
            return response;
        }
    }
}
