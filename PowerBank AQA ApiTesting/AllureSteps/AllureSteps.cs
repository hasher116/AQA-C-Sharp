using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using PowerBank_AQA_ApiTestingCore.ApiRequest;
using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_ApiTestingCore.Models;
using System.Net;

namespace PowerBank_AQA_ApiTesting.AllureSteps
{
    public static class ApiAllureSteps
    {
        [AllureStep("Add Headers")]
        public static Dictionary<string, string> CreateHeaders()
        {
            return RequestCreator.CreateHeaders(HeaderTypes.JSON);
        }

        [AllureStep("Create request")]
        public static RequestInfo CreateRequest<T>(T requestBody, Dictionary<string, string> headers, string Url,
            HttpMethod method, int timeout)
            where T : class
        {
            return RequestCreator.CreateRequest(requestBody, headers, Url, method, timeout);
        }

        [AllureStep("Send message")]
        public static ResponseInfo SendMessage(this WebService service, RequestInfo request)
        {
            return service.SendMessageAsync(request).Result;
        }

        [AllureStep("Get value")]
        public static string GetValueByKee(this JToken token, string kee)
        {
            return token.GetJsonValueByKey(kee);
        }

        [AllureStep("Check code #{0} #{1}")]
        public static void CheckStatusCode(this HttpStatusCode httpCode, int code)
        {
            ((int)httpCode).Should().Be(code, $"Статус код отличается от {code}");
        }

        [AllureStep("Create request")]
        public static RequestInfo CreateRequest(Dictionary<string, string> headers, string Url,
       HttpMethod method, int timeout)

        {
            var request = new RequestInfo
            {
                Headers = headers,
                Url = Url,
                Method = method,
                Timeout = timeout
            };
            return request;
        }
    }
}
