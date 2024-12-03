using System.Text.RegularExpressions;

namespace PowerBank_AQA_UITestingCore.Extensions
{
    public static class StringExtensions
    {
        public static string GetStringByRegex(this string defaultString, string regex = "[^/.](.*)")
        {
            var str = defaultString;
            str = Regex.Match(str, regex).Value.Trim();
            return str;
        }
    }
}
