using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PowerBank_AQA_ApiTestingCore.Models
{
    public record ResponseInfo
    {
        public RequestInfo Request { get; init; }
        public HttpStatusCode StatusCode { get; init; }
        public HttpResponseHeaders Headers { get; init; }
        public HttpContentHeaders ContentHeaders { get; init; }
        public HttpContent Content { get; init; }
        public JToken ResponseBody { get; init; }
    }
}
