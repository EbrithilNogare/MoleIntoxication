using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ColorHelper
{
    public static float DistanceRGB(Color a, Color b)
    {
        return Vector3.Distance(a.ToVector3(), b.ToVector3());
    }
    public static float DistanceRGBA(Color a, Color b)
    {
        return Vector4.Distance(a.ToVector4(), b.ToVector4());
    }

    public static Color WeightedAverage(params (Color c, float w)[] data)
    {
        var colorSum = data.Sum(x => x.c.ToVector3() * x.w);
        var weigthSum = data.Sum(x => x.w);
        return (colorSum / weigthSum).ToColor();
    }

    public static Vector3 ToVector3(this Color c)
    {
        return new Vector3(c.r, c.g, c.b);
    }

    public static Vector4 ToVector4(this Color c)
    {
        return new Vector4(c.r, c.g, c.b, c.a);
    }

    public static Color ToColor(this Vector3 c)
    {
        return new Color(c.x, c.y, c.z);
    }

    public static Color ToColor(this Vector4 c)
    {
        return new Color(c.x, c.y, c.z, c.w);
    }
}
