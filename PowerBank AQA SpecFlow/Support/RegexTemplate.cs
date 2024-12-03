using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerBank_AQA_SpecFlow.Support
{
    public static class RegexTemplate
    {
        public static string GetNumberSelection(string str)
        {
            return Regex.Replace(str, "[А-Яа-я A-Za-z%₽]", "");
        }

        public static string GetStringWithoutSpace(string str)
        {
            return Regex.Replace(str, " ", "");
        }
    }
}
