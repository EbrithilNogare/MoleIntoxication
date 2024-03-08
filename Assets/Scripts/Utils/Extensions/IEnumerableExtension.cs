using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace System.Linq
{
    public static class IEnumerableExtension
    {
        public static Vector2 Sum<TElement>(this IEnumerable<TElement> source, Func<TElement, Vector2> selector)
        => source.Select(selector).Sum();
        public static Vector3 Sum<TElement>(this IEnumerable<TElement> source, Func<TElement, Vector3> selector)
            => source.Select(selector).Sum();
        public static Vector4 Sum<TElement>(this IEnumerable<TElement> source, Func<TElement, Vector4> selector)
            => source.Select(selector).Sum();
        public static Vector2Int Sum<TElement>(this IEnumerable<TElement> source, Func<TElement, Vector2Int> selector)
            => source.Select(selector).Sum();
        public static Vector3Int Sum<TElement>(this IEnumerable<TElement> source, Func<TElement, Vector3Int> selector)
            => source.Select(selector).Sum();

        public static Vector2 Sum(this IEnumerable<Vector2> vectors)
        {
            var res = Vector2.zero;
            foreach (var vector in vectors)
                res += vector;
            return res;
        }

        public static Vector3 Sum(this IEnumerable<Vector3> vectors)
        {
            var res = Vector3.zero;
            foreach (var vector in vectors)
                res += vector;
            return res;
        }

        public static Vector4 Sum(this IEnumerable<Vector4> vectors)
        {
            var res = Vector4.zero;
            foreach (var vector in vectors)
                res += vector;
            return res;
        }

        public static Vector2Int Sum(this IEnumerable<Vector2Int> vectors)
        {
            var res = Vector2Int.zero;
            foreach (var vector in vectors)
                res += vector;
            return res;
        }

        public static Vector3Int Sum(this IEnumerable<Vector3Int> vectors)
        {
            var res = Vector3Int.zero;
            foreach (var vector in vectors)
                res += vector;
            return res;
        }
    }
}
