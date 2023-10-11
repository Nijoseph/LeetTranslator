using System;

namespace LeetTranslator.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TrimToLengthWithEllipsis(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : $"{value.Substring(0, maxLength - 3)}...";
        }
    }
}
