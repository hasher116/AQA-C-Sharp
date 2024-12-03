using PowerBank_AQA_ApiTestingCore.Infrastructures;
using PowerBank_AQA_ApiTestingCore.Models;
using System.Net.Http.Json;

namespace PowerBank_AQA_ApiTestingCore.ApiRequest
{
    public static class RequestCreator
    {
        public static RequestInfo CreateRequest<T>(T requestBody, Dictionary<string, string> headers, string Url,
            HttpMethod method, int timeout)
            where T : class
        {
            var request = new RequestInfo
            {
                Content = JsonContent.Create(requestBody),
                Headers = headers,
                Url = Url,
                Method = method,
                Timeout = timeout
            };
            return request;
        }

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

        public static RequestInfo CreateTextRequest(string requestBody, Dictionary<string, string> headers, string Url,
           HttpMethod method, int timeout)
        {
            var request = new RequestInfo
            {
                Content = new StringContent(requestBody),
                Headers = headers,
                Url = Url,
                Method = method,
                Timeout = timeout
            };
            return request;
        }

        public static Dictionary<string, string> CreateHeaders(HeaderTypes type)
        {
            switch (type)
            {
                case HeaderTypes.JSON:
                    {
                        return new Dictionary<string, string>
                        {
                            { "Content-Type", DefaultContentType.JSON  },
                            { "accept",  DefaultContentType.JSON  }
                        };
                    }

                case HeaderTypes.XML:
                    {
                        return new Dictionary<string, string>
                        {
                            { "Content-Type", DefaultContentType.XML },
                            { "accept", DefaultContentType.XML  }
                        };
                    }

                case HeaderTypes.TEXT:
                    {
                        return new Dictionary<string, string>
                        {
                            { "Content-Type", DefaultContentType.TEXT },
                            { "accept", DefaultContentType.TEXT  }
                        };
                    }

                default: return null;
            }
        }
    }
}
