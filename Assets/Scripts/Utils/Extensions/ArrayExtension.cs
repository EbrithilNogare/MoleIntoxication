using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Tools
{
    public static class ArrayExtension
    {
        /// <summary>
        /// Extension method for converting a one-dimensional array to a two-dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the input array</typeparam>
        /// <param name="input">The input one-dimensional array</param>
        /// <param name="width">The width of the two-dimensional array</param>
        /// <param name="height">The height of the two-dimensional array</param>
        /// <returns>A two-dimensional array with the same elements as the input array, arranged according to the specified width and height</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="input"/> is null</exception>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="width"/> or <paramref name="height"/> is less than or equal to 0</exception>
        public static T[,] To2DArray<T>(this T[] input, int width, int height)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (width <= 0)
                throw new ArgumentException(nameof(width));
            if (height <= 0)
                throw new ArgumentException(nameof(height));

            T[,] output = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }
    }
}
