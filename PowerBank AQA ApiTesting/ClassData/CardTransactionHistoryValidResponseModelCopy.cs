using Newtonsoft.Json;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class CardTransactionHistoryValidResponseModelCopy
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("time", Required = Required.Always)]
        public string Time { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public string Date { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public double Amount { get; set; }

        [JsonProperty("comment", Required = Required.Always)]
        public string Comment { get; set; }

        [JsonProperty("currencyCode", Required = Required.Always)]
        public string CurrencyCode { get; set; }
    }
}
