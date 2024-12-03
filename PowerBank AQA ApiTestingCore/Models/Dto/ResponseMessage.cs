using Newtonsoft.Json.Linq;
using System.Net;

namespace PowerBank_AQA_ApiTestingCore.Models.Dto
{
    public class ResponseMessage
    {
        public string Url { get; init; }
        public string Method { get; init; }
        public string Content { get; init; }
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; init; }
        public HttpStatusCode StatusCode { get; init; }

        public override string ToString()
        {
            return $@"Url: {Url}{Environment.NewLine}Method: {Method}{Environment.NewLine}
                    Headers: {JContainer.FromObject(Headers)}{Environment.NewLine}StatusCode: {StatusCode.ToString()}{Method}{Environment.NewLine}Content: {Content}";
        }
    }
}
