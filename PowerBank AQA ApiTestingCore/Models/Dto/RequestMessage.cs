using Newtonsoft.Json.Linq;

namespace PowerBank_AQA_ApiTestingCore.Models.Dto
{
    public class RequestMessage
    {
        public string Url { get; init; }
        public string Method { get; init; }
        public string? Content { get; set; }

        public Dictionary<string, string> Headers { get; init; }

        public override string ToString()
        {
            return $@"Url: {Url}{Environment.NewLine}Method: {Method}{Environment.NewLine}
                    Headers: {JContainer.FromObject(Headers)}{Environment.NewLine}Content: {Content}";
        }
    }
}
