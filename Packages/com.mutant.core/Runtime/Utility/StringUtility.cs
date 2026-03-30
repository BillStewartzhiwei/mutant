using System;

namespace Bill.Mutant.Core
{
    public static class StringUtility
    {
        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string NullIfWhiteSpace(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public static string Truncate(string value, int maxLength, string suffix = "...")
        {
            if (value == null) return null;
            if (maxLength < 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (value.Length <= maxLength) return value;

            suffix ??= string.Empty;
            if (maxLength <= suffix.Length) return suffix.Substring(0, maxLength);

            return value.Substring(0, maxLength - suffix.Length) + suffix;
        }
    }
}
