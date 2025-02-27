﻿using System;

namespace PowerBank_AQA_ApiTesting.Extensions
{
    public static class StringExtensions
    {
        public static string UpperFirstChar(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
