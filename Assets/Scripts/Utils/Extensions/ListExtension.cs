using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    /// <summary>
    /// Extension method for getting a random element from a list.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list</typeparam>
    /// <param name="list">The input list</param>
    /// <returns>A random element from the list</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="list"/> is null</exception>
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null)
            throw new System.ArgumentNullException(nameof(list));

        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Extension method for getting a random element from an array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array</typeparam>
    /// <param name="array">The input array</param>
    /// <returns>A random element from the array</returns>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
    public static T GetRandom<T>(this T[] array)
    {
        if (array == null)
            throw new System.ArgumentNullException(nameof(array));

        return array[Random.Range(0, array.Length)];
    }
}
