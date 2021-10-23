using System;
using System.Collections.Generic;

namespace CatApi.utils
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> mapper)
        {
            foreach (var item in source)
            {
                mapper(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> mapper)
        {
            var i = 0;
            foreach (var item in source)
            {
                mapper(item, i++);
            }
        }
    }
}