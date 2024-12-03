using PowerBank_AQA_ApiTestingCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PowerBank_AQA_ApiTestingCore.Extensions
{
    public static class JsonExtensions
    {
        public static JToken ResponseToJson(this string jsonAsString)
        {
            return JToken.Parse(JsonConvert.SerializeObject(jsonAsString));
        }

        public static T? DeserializeModel<T>(ResponseInfo response)
        {
            return response.ResponseBody.ToObject<T>();
        }

        public static List<T?> DeserializeListModel<T>(ResponseInfo response)
        {
            return JsonConvert.DeserializeObject<List<T>>(response.ResponseBody.ToString());
        }
    }
}
