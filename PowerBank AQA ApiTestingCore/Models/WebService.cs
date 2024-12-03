using PowerBank_AQA_ApiTestingCore.Helpers;
using PowerBank_AQA_ApiTestingCore.Models.Interfaces;
using PowerBank_AQA_ApiTestingCore.Models.Provider;
using PowerBank_AQA_TestingCore.Helpers;
using Microsoft.Extensions.Logging;
using NUnit.Allure.Attributes;

namespace PowerBank_AQA_ApiTestingCore.Models
{
    public sealed class WebService : IWebService, IDisposable
    {
        private readonly AsyncLocal<IHttpProvider> httpProvider = new() { Value = null };
        public WebService()
        {
            Http = new HttpProvider();
        }

        public IHttpProvider Http
        {
            get => httpProvider.Value;
            set => httpProvider.Value = value;
        }

        public async Task<ResponseInfo> SendMessageAsync(RequestInfo requestInfo)
        {
            var isValid = Validate.ValidateUrl(requestInfo.Url);
            if (isValid)
            {
                Log.Logger().LogInformation(await requestInfo.CreateMessageSync());

                var response = await Http.SendRequestAsync(requestInfo);

                var responseInfo = new ResponseInfo
                {
                    Headers = response.Headers,
                    StatusCode = response.StatusCode,
                    Content = response.Content,
                    Request = requestInfo,
                    ContentHeaders = response.Content.Headers,
                    ResponseBody = response.Content.ReadAsStringAsync().Result.ToJson()
                };

                Log.Logger().LogInformation(await responseInfo.CreateMessageSync());

                return responseInfo;
            }

            Log.Logger().LogError($"URL {requestInfo.Url} не валидный.");
            return null;
        }

        public async Task<ResponseInfo> SendMessageAsyncWithoutResponseBody(RequestInfo requestInfo)
        {
            var isValid = Validate.ValidateUrl(requestInfo.Url);
            if (isValid)
            {
                var response = await Http.SendRequestAsync(requestInfo);

                var responseInfo = new ResponseInfo
                {
                    Headers = response.Headers,
                    StatusCode = response.StatusCode,
                    Content = response.Content,
                    Request = requestInfo,
                    ContentHeaders = response.Content.Headers,
                };

                return responseInfo;
            }

            Log.Logger().LogError($"URL {requestInfo.Url} не валидный.");
            return null;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
