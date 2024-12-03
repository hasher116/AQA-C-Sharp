using System;

namespace PowerBank_AQA_ApiTestingCore.Helpers
{
    public static class Validate
    {
        public static bool ValidateUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
