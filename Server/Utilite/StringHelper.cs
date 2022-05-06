using System;
using Server.Network;
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

        public static NetworkPacket ToPacket(this byte[] buffer, int extraBytes = 0)
        {
            return new NetworkPacket(1 + extraBytes, buffer);
        }

        public static void BuildBufferWithOpcodePacket(this List<byte> bytes, byte[] buffer)
        {
            bytes.AddRange(BitConverter.GetBytes((short)(buffer.Length + 2)));
            bytes.AddRange(buffer);
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
