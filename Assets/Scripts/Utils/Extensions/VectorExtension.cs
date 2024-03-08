using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Tools
{
    /// <summary>
    /// An enum for specifying options for range comparison.
    /// </summary>
    [Flags]
    public enum RangeCheckOptions
    {
        /// <summary>
        /// Close interval
        /// </summary>
        Close = 0,
        /// <summary>
        /// Semi open interval
        /// </summary>
        OpenBegin = 0x01,
        /// <summary>
        /// Semi closed interval
        /// </summary>
        OpenEnd = 0x02,
        /// <summary>
        /// Open interval
        /// </summary>
        Open = OpenBegin | OpenBegin,
    }

    public static class VectorExtension
    {
        /// <summary>
        /// Extension method for checking whether a vector's x and y values fall within a specified range.
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <param name="value">The value to compare the vector's x and y values to</param>
        /// <param name="rangeCheckOptions">Options for specifying the type of range comparison to perform</param>
        /// <returns>True if the vector's x and y values fall within the specified range, false otherwise</returns>
        public static bool RangeCheck(this Vector2 vec, float value, RangeCheckOptions rangeCheckOptions = RangeCheckOptions.Open)
        {
            var open = vec.x > value && vec.y > value;
            var begin = rangeCheckOptions.HasFlag(RangeCheckOptions.OpenBegin) || vec.x == value;
            var end = rangeCheckOptions.HasFlag(RangeCheckOptions.OpenEnd) || vec.y == value;
            return open || begin || end;
        }

        /// <summary>
        /// Extension method for checking whether a vector's x and y values fall within a specified range.
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <param name="value">The value to compare the vector's x and y values to</param>
        /// <param name="rangeCheckOptions">Options for specifying the type of range comparison to perform</param>
        /// <returns>True if the vector's x and y values fall within the specified range, false otherwise</returns>
        public static bool RangeCheck(this Vector2Int vec, int value, RangeCheckOptions rangeCheckOptions = RangeCheckOptions.Open)
        {
            var open = vec.x > value && vec.y > value;
            var begin = rangeCheckOptions.HasFlag(RangeCheckOptions.OpenBegin) || vec.x == value;
            var end = rangeCheckOptions.HasFlag(RangeCheckOptions.OpenEnd) || vec.y == value;
            return open || begin || end;
        }

        public static Vector2 Clamp(this Vector2 vec, Vector2 min, Vector2 max)
        {
            return new Vector2(Mathf.Clamp(vec.x, min.x, max.x), Mathf.Clamp(vec.y, min.y, max.y));
        }
    }
}
