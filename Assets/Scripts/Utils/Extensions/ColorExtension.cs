using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Tools
{
    public static class ColorExtension
    {
        /// <summary>
        /// Extension method for determining whether two colors are close to each other.
        /// </summary>
        /// <param name="a">The first color</param>
        /// <param name="b">The second color</param>
        /// <param name="maximalDistance">The maximum distance between the two colors for them to be considered close (defaults to 0.05)</param>
        /// <returns>True if the distance between the two colors is less than the specified maximal distance, false otherwise</returns>
        public static bool IsCloseTo(this Color a, Color b, float maximalDistance = 0.05f)
        {
            var va = new Vector3(a.r, a.g, a.b);
            var vb = new Vector3(b.r, b.g, b.b);
            return Vector3.Distance(va, vb) < maximalDistance;
        }

        /// <summary>
        /// Extension method for creating a new color with a modified red component.
        /// </summary>
        /// <param name="c">The input color</param>
        /// <param name="r">The new value for the red component</param>
        /// <returns>A new color with the modified red component</returns>
        public static Color WithR(this Color c, float r)
        {
            return new Color(r, c.g, c.b, c.a);
        }

        /// <summary>
        /// Extension method for creating a new color with a modified green component.
        /// </summary>
        /// <param name="c">The input color</param>
        /// <param name="g">The new value for the green component</param>
        /// <returns>A new color with the modified green component</returns>
        public static Color WithG(this Color c, float g)
        {
            return new Color(c.r, g, c.b, c.a);
        }

        /// <summary>
        /// Extension method for creating a new color with a modified blue component.
        /// </summary>
        /// <param name="c">The input color</param>
        /// <param name="b">The new value for the blue component</param>
        /// <returns>A new color with the modified blue component</returns>
        public static Color WithB(this Color c, float b)
        {
            return new Color(c.r, c.g, b, c.a);
        }

        /// <summary>
        /// Extension method for creating a new color with a modified alpha component.
        /// </summary>
        /// <param name="c">The input color</param>
        /// <param name="a">The new value for the alpha component</param>
        /// <returns>A new color with the modified alpha</returns>
        public static Color WithAplha(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
    }
}
