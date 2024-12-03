using System;
using NUnit.Allure.Attributes;

namespace PowerBank_AQA_ApiTestingCore.Extensions
{
    public static class HeadersExtensions
    {
        public static void AddHeader(this Dictionary<string, string> existingHeaders,
            (string, string) newHeader)
        {
            existingHeaders.Add(newHeader.Item1, newHeader.Item2);
        }

        public static Dictionary<string, string> AddHeader(this Dictionary<string, string> existingHeaders,
            Dictionary<string, string> newHeader)
        {
            return existingHeaders.Concat(newHeader).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
