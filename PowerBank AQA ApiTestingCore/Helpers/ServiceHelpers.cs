using System.Net.Http.Headers;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;

namespace PowerBank_AQA_ApiTestingCore.Helpers
{
    public static class ServiceHelpers
    {
        public static void AddHeaders<T>(this T clientOrRequest, Dictionary<string, string> headers)
            where T : HttpHeaders
        {
            var _clientOrRequest = clientOrRequest;
            if (headers == null)
            {
                return;
            }

            foreach (var (name, value) in headers)
            {
                _clientOrRequest.TryAddWithoutValidation(name, value);
            }
        }

        public static JToken ToJson(this object obj)
        {
            if (obj == null)
            {
                Log.Logger().LogInformation(nameof(obj), $"{nameof(obj)} is null");
                throw new ArgumentNullException(nameof(obj), $"{nameof(obj)} is null");
            }

            if ((string)obj == string.Empty)
            {
                Log.Logger().LogInformation("Пришел пустой ResponseBody");
                return null;
            }

            return JToken.Parse(obj.ToString()!);
        }

        public static string GetJsonValueByKey(this JToken token, string key)
        {
            return token.SelectToken(key)?.ToString()!;
        }
    }
}
