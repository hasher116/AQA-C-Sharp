using Newtonsoft.Json;
using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class CardTransactions
    {
        [JsonProperty("date", Required = Required.Always)]
        public DateTime Date { get; set; }

        [JsonProperty("time", Required = Required.Always)]
        public string Time { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("purpose", Required = Required.Always)]
        public string Purpose { get; set; }

        [JsonProperty("amount", Required = Required.Always)]
        public string Amount { get; set; }
    }
}
