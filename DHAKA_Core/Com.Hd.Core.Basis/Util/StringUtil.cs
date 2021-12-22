#region

using System;

#endregion

namespace Com.Hd.Core.Basis.Util
{
    internal static class StringUtil
    {
        /*
         * Determines if two strings are equal. Treats null and String.Empty as equivalent.
         */
        internal static bool EqualsNE(string s1, string s2)
        {
            if (s1 == null)
            {
                s1 = string.Empty;
            }

            if (s2 == null)
            {
                s2 = string.Empty;
            }

            return string.Equals(s1, s2, StringComparison.Ordinal);
        }

        /*
         * Determines if two strings are equal, ignoring case.
         */
        internal static bool EqualsIgnoreCase(string s1, string s2)
        {
            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        /*
       * Determines if the first string starts with the second string, ignoring case.
       */
        internal static bool StartsWith(string s1, string s2)
        {
            if (s2 == null)
            {
                return false;
            }

            return 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.Ordinal);
        }

        /*
         * Determines if the first string starts with the second string, ignoring case.
         */
        internal static bool StartsWithIgnoreCase(string s1, string s2)
        {
            if (s2 == null)
            {
                return false;
            }

            return 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
        }

        internal static string[] ObjectArrayToStringArray(object[] objectArray)
        {
            string[] stringKeys = new string[objectArray.Length];
            objectArray.CopyTo(stringKeys, 0);
            return stringKeys;
        }

    }
}