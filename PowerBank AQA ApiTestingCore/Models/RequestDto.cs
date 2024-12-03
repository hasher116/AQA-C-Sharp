using PowerBank_AQA_ApiTestingCore.Infrastructures;
using PowerBank_AQA_ApiTestingCore.Extensions;

namespace PowerBank_AQA_ApiTestingCore.Models
{
    public class RequestDto
    {
        private readonly IEnumerable<Header> headers;

        public RequestDto(IEnumerable<Header> headers)
        {
            this.headers = headers;
        }

        public Dictionary<string, string> Header => SetData(HeaderType.HEADER);

        public Dictionary<string, string> Query => SetData(HeaderType.QUERY);

        //public ICredentials Credentials => headers.CheckParameter(HeaderType.CREDENTIAL)

        public HttpContent Content { get; }

        public int Timeout => headers.CheckParameter(HeaderType.TIMEOUT) ? GetTimeout() : Constants.DEFAULT_TIMEOUT;

        private Dictionary<string, string> SetData(HeaderType headerType)
        {
            return headers
                .Where(h => h.Style == headerType)
                .ToDictionary(e => e.Name, e => e.Value);
        }

        private string GetData(HeaderType headerType)
        {
            var data = headers
                .Where(h => h.Style == headerType)
                .ToDictionary(e => e.Name, e => e.Value);

            if (data.Any())
            {
                return data.Values.FirstOrDefault();
            }

            return null;
        }

        private int GetTimeout()
        {
            var headerValue = headers.FirstOrDefault(h => h.Style == HeaderType.TIMEOUT)?.Value;
            bool isParsed = int.TryParse(headerValue, out var value);
            if (isParsed)
            {
                return value;
            }
            else
            {
                throw new FormatException($"{headerValue} не удалось преобразовать в int");
            }
        }

        private string GetBody()
        {
            var name = headers.FirstOrDefault(h => h.Style == HeaderType.BODY)?.Value;
            return name;
        }

        //private ICredentials GetCredentials()
        //{
        //    var name = headers.FirstOrDefault(h => h.Style != HeaderType.CREDENTIAL)?.Value;
        //    return name as ICredentials;
        //}
    }
}
