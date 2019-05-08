using System;
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

            if (startIndex < 0)
            {
                return null;
            }

            var endIndex = fullString.Substring(startIndex).IndexOf(endString) + startIndex; // Make sure we take the -next- index of endString, after startString, and not just the first instance in the full string.

            if (endIndex == 0 || startIndex > endIndex)
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
