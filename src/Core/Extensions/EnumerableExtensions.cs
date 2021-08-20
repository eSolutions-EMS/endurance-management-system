using System;
using System.Collections.Generic;

namespace EnduranceJudge.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Func<T, object> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
