using System;
using System.Collections.Generic;

namespace Server.Utilite
{
    public static class StringHelper
    {
        public static bool EqualsIgnoreCase(this string str, string stringToCompare)
        {
            return str.Equals(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EqualsMatchCase(this string str, string stringToCompare)
        {
            return str.Equals(stringToCompare, StringComparison.InvariantCulture);
        }

        public static bool StartsWithIgnoreCase(this string str, string stringToCompare)
        {
            return str.StartsWith(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool StartsWithMatchCase(this string str, string stringToCompare)
        {
            return str.StartsWith(stringToCompare, StringComparison.InvariantCulture);
        }

        public static bool EndsWithIgnoreCase(this string str, string stringToCompare)
        {
            return str.EndsWith(stringToCompare, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EndsWithMatchCase(this string str, string stringToCompare)
        {
            return str.EndsWith(stringToCompare, StringComparison.InvariantCulture);
        }

        public static SortedList<TKey, TValue> ToSortedList<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            SortedList<TKey, TValue> ret = new SortedList<TKey, TValue>();

            foreach (TSource item in source)
                ret.Add(keySelector(item), valueSelector(item));

            return ret;
        }
    }
}
