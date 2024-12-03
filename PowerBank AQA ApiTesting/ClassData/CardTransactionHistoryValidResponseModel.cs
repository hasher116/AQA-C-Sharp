using Newtonsoft.Json;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class CardTransactionHistoryValidResponseModel
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("time", Required = Required.Always)]
        public string Time { get; set; }

        [JsonProperty("accountNumber", Required = Required.Always)]
        public string AccountNumber { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty("cardNumber", Required = Required.Always)]
        public string CardNumber { get; set; }

        [JsonProperty("comment", Required = Required.Always)]
        public string Comment { get; set; }

        [JsonProperty("emailBillInfo", Required = Required.Always)]
        public string EmailBillInfo { get; set; }

        [JsonProperty("currencyCode", Required = Required.Always)]
        public string CurrencyCode { get; set; }
    }
}
