using PowerBank_AQA_ApiTestingCore.Models;
using PowerBank_AQA_ApiTestingCore.Models.Dto;
using System.Net.Http.Json;
using System.Text;

namespace PowerBank_AQA_ApiTestingCore.Helpers
{
    public static class CreateMessage
    {
        public static async Task<string> CreateMessageSync(this RequestInfo request)
        {
            var str = new StringBuilder();
            str.Append("Request:");
            str.Append(Environment.NewLine);
            string content;

            switch (request.Content)
            {
                case StringContent stringContent:
                    {
                        content = (await stringContent.ReadAsStringAsync()).ReplaceLineEndings();
                        break;
                    }

                case MultipartFormDataContent formDataContent:
                    {
                        var stream = await formDataContent.ReadAsStringAsync();
                        content = $"{stream.Length} bytes";
                        break;
                    }

                case JsonContent jsonContent:
                    {
                        content = (await jsonContent.ReadAsStringAsync()).ReplaceLineEndings();
                        break;
                    }

                default:
                    {
                        content = null;
                        break;
                    }
            }

            str.Append(new RequestMessage
            {
                Url = request.Url,
                Headers = request.Headers,
                Method = request.Method.Method,
                Content = content
            });

            return str.ToString();
        }

        public static async Task<string> CreateMessageSync(this ResponseInfo response)
        {
            var str = new StringBuilder();
            str.Append("Response:");
            str.Append(Environment.NewLine);
            str.Append(new ResponseMessage
            {
                Url = response.Request.Url,
                Method = response.Request.Method.Method,
                Headers = response.Headers.Concat(response.ContentHeaders),
                StatusCode = response.StatusCode,
                Content = await response.Content.ReadAsStringAsync()
            });

            return str.ToString();
        }
    }
}
