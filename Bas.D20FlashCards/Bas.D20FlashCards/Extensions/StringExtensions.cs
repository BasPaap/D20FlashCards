using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bas.D20FlashCards.Extensions
{
    static class StringExtensions
    {
        /// <summary>
        /// Returns the substring between the first occurrence of <paramref name="startString"/> and <paramref name="endString"/>.
        /// </summary>
        /// <param name="fullString">The string to be searched.</param>
        /// <param name="startString">The string value after which the substring is to start.</param>
        /// <param name="endString">The string value before which the substring is to end.</param>
        /// <returns>The substring between the first occurrence of <paramref name="startString"/> and <paramref name="endString"/>.</returns>
        public static string Substring(this string fullString, string startString, string endString)
        {
            // Find the indexof startString, if available.
            var startIndex = fullString.IndexOf(startString);
            if (startIndex < 0)
            {
                return null;
            }

            // Find the indexof endString, if available.
            // Make sure we take the -next- index of endString, after startString, and not just the first instance in the full string.
            var endIndex = fullString.Substring(startIndex).IndexOf(endString) + startIndex; 
            if (endIndex == 0 || startIndex > endIndex)
            {
                return null;
            }

            // Calculate the index and length of the substring to be found.
            var substringIndex = startIndex + startString.Length;
            var substringLength = endIndex - substringIndex;

            // Return the found substring.
            var substring = fullString.Substring(substringIndex, substringLength);
            Debug.Assert(substring.Length == substringLength);
            return substring;
        }
    }
}
