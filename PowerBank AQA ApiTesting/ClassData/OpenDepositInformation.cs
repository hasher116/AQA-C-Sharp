using Newtonsoft.Json;
using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class OpenDepositInformation
    {
        [JsonProperty("accountNumber", Required = Required.Always)]
        public string AccountNumber { get; set; }

        [JsonProperty("depositProductName", Required = Required.Always)]
        public string DepositProductName { get; set; }

        [JsonProperty("initialOpenDate", Required = Required.Always)]
        public DateTime InitialOpenDate { get; set; }

        [JsonProperty("initialCloseDate", Required = Required.Always)]
        public DateTime InitialCloseDate { get; set; }

        [JsonProperty("currentBalance", Required = Required.Always)]
        public double CurrentBalance { get; set; }

        [JsonProperty("currencyCode", Required = Required.Always)]
        public string CurrencyCode { get; set; }

        [JsonProperty("imageLink", Required = Required.Always)]
        public string ImageLink { get; set; }

    }
}
