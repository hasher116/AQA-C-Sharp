using PowerBank_AQA_ApiTestingCore.Infrastructures;

namespace PowerBank_AQA_ApiTestingCore.Models
{
    public record Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public HeaderType Style { get; set; }
    }
}
