using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PowerBank_AQA_ApiTestingCore.Models
{
    public class RequestInfo
    {
        [Required(ErrorMessage = "URL является обязательным параметром")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Headers является обязательным параметром")]
        public Dictionary<string, string> Headers { get; set; }

        [Required(ErrorMessage = "Method является обязательным параметром")]
        public HttpMethod Method { get; set; }

        [Required(ErrorMessage = "Content является обязательным параметром")]
        public HttpContent Content { get; set; }

        public ICredentials Credentials { get; set; }

        [Required(ErrorMessage = "Timeout является обязательным параметром")]
        public int Timeout { get; init; }
    }
}
