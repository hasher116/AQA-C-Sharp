using PowerBank_AQA_ApiTestingCore.Helpers;
using System.Net.Http.Headers;
using System.Text;

namespace PowerBank_AQA_ApiTestingCore.Models.Provider
{
    public class HttpProvider : IHttpProvider, IDisposable
    {
        public async Task<HttpResponseMessage> SendRequestAsync(RequestInfo request)
        {
            var handlers = CreateHandler(request);
            using var httpClient = new HttpClient(handlers)
            {
                BaseAddress = new Uri(request.Url),
                Timeout = TimeSpan.FromSeconds(request.Timeout)
            };
            httpClient.DefaultRequestHeaders.AddHeaders(request.Headers);

            var httpRequestMessage = new HttpRequestMessage
            {
                Content = request.Content,
                Method = request.Method
            };

            var contentType = request.Headers.FirstOrDefault(pair => pair.Key.ToLower() == "content-type");
            if (!string.IsNullOrEmpty(contentType.Key) && !string.IsNullOrEmpty(contentType.Value))
            {
                if (httpRequestMessage.Content is not null)
                {
                    httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType.Value);
                    var requestContent = httpRequestMessage.Content.ReadAsStringAsync().Result.ToString();
                    ASCIIEncoding encoding = new();
                    var length = encoding.GetBytes(requestContent).Length;
                    httpRequestMessage.Content.Headers.ContentLength = length;
                }
            }

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private HttpClientHandler CreateHandler(RequestInfo request)
        {
            var handler = new HttpClientHandler();

            if (request.Credentials != null)
            {
                handler.Credentials = request.Credentials;
            }

            return handler;
        }
    }
}
