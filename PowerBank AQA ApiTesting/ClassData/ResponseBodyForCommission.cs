using Newtonsoft.Json;

namespace PowerBank_AQA_ApiTesting.ClassData
{
    public class ResponseBodyForCommission
    {
        [JsonProperty("minAmountCommission", Required = Required.Always)]
        public double MinAmountCommission { get; set; }
        [JsonProperty("percentCommission", Required = Required.Always)]
        public double PercentCommission { get; set; }
        [JsonProperty("minAmountTransaction", Required = Required.Always)]
        public double MinAmountTransaction { get; set; }
    }
}
