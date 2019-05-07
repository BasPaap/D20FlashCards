﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bas.D20FlashCards.Extensions
{
    static class StringExtensions
    {
        public static string Substring(this string fullString, string startString, string endString)
        {            
            var startIndex = fullString.IndexOf(startString);
            var endIndex = fullString.IndexOf(endString);

            if (startIndex < 0 || endIndex == 0 || startIndex > endIndex)
            {
                return null;
            }

            var substringIndex = startIndex + startString.Length;
            var substringLength = endIndex - substringIndex;

            var substring = fullString.Substring(substringIndex, substringLength);
            Debug.Assert(substring.Length == substringLength);

            return substring;
        }
    }
}
