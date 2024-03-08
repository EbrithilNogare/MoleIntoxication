using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Tools
{
    public static class RandomHelper
    {
        /// <summary>
        /// Extension method for generating a random 2D unit vector.
        /// </summary>
        /// <returns>A random 2D unit vector</returns>
        public static Vector2 Vector2()
        {
            var vec = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            return vec.normalized;
        }


        /// <summary>
        /// Extension method for generating a random 2D int vector.
        /// </summary>
        /// <returns>A random 2D unit vector</returns>
        public static Vector2Int Vector2Int()
        {
            var vec = new Vector2Int(Random.Range(0, 2), Random.Range(0, 2));
            return vec;
        }

        /// <summary>
        /// Extension method for generating a random 3D unit vector.
        /// </summary>
        /// <returns>A random 3D unit vector</returns>
        public static Vector3 Vector3()
        {
            var vec = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            return vec.normalized;
        }


        /// <summary>
        /// Extension method for generating a random 3D int vector.
        /// </summary>
        /// <returns>A random 3D int vector</returns>
        public static Vector3Int Vector3Int()
        {
            var vec = new Vector3Int(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
            return vec;
        }

        /// <summary>
        /// Extension method for generating a random 2D int vector.
        /// </summary>
        /// <returns>A random 2D unit vector</returns>
        public static Vector2Int FromRange(Vector2Int min, Vector2Int max)
        {
            var vec = new Vector2Int(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            return vec;
        }

        public static float From(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }
    }
}