using PowerBank_AQA_ApiTestingCore.Infrastructures;
using PowerBank_AQA_ApiTestingCore.Models;

namespace PowerBank_AQA_ApiTestingCore.Extensions
{
    public static class HeaderExtensions
    {
        public static bool CheckParameter(this IEnumerable<Header> headers, HeaderType parameter)
        {
            return headers.Count(h => h.Style == parameter) == 1;
        }
    }
}
