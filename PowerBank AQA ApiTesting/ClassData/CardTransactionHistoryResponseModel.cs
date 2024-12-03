using Newtonsoft.Json;
using System;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class CardTransactionHistoryResponseModel
    {
        [JsonProperty("cardTransactions", Required = Required.Always)]
        public CardTransactionHistoryValidResponseModelCopy[] CardTransactions { get; set; }

        [JsonProperty("pagination", Required = Required.Always)]
        public Pagination Pagination { get; set; }

    }
}
