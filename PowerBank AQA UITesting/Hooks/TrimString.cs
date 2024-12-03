using System;
using static System.Net.Mime.MediaTypeNames;

namespace PowerBank_AQA_UITesting.Hooks
{
    public static class TrimString
    {
        private static string str;

        public static string GetNumbersInString(string strForTrim)
        {
            if (strForTrim.Contains("год") || strForTrim.Contains("лет"))
            {
                str = new string(strForTrim.Where(t => char.IsDigit(t)).ToArray());
                var strInMonths = Convert.ToInt32(str) * 12;
                return strInMonths.ToString();
            }
            return new string(strForTrim.Where(t => char.IsDigit(t)).ToArray());
        }

        public static string GetCheckedString(string stringForCheck, string comparator)
        {
            if (stringForCheck.Contains(comparator))
                return comparator;
            return stringForCheck;
        }
        
        public static bool ReturnBoolFromString(string stringForCheck)
        {
            if (stringForCheck.Contains("Да") || stringForCheck.Contains("Предусмотрено") || stringForCheck.Contains("Действующий"))
                return true;
            return false;
        }

        public static string GetTrimPercent(string strForTrim)
        {
            Console.WriteLine(strForTrim.Substring(0, 3));
            return strForTrim.Substring(0,3);
        }
    }
}
