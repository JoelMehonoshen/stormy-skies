using UnityEngine;
using System.Collections;

public static class Extensions
{
    // Vector3 Extensions
    public static Vector3 ToVector3(this Vector2 a_vector2)
    {
        return new Vector3(a_vector2.x, a_vector2.y, 0f);
    }

    public static Vector3 SetX(this Vector3 a_vector, float a_newX)
    {
        a_vector.x = a_newX;
        return a_vector;
    }

    public static Vector3 SetY(this Vector3 a_vector, float a_newY)
    {
        a_vector.y = a_newY;
        return a_vector;
    }

    public static Vector3 SetZ(this Vector3 a_vector, float a_newZ)
    {
        a_vector.z = a_newZ;
        return a_vector;
    }

    // Vector2 Extensions
    public static Vector2 ToVector2(this Vector3 a_vector3)
    {
        return new Vector2(a_vector3.x, a_vector3.y);
    }

    public static Vector2 SetX(this Vector2 a_vector, float a_newX)
    {
        return new Vector2(a_newX, a_vector.y);
    }

    public static Vector2 SetY(this Vector2 a_vector, float a_newY)
    {
        return new Vector2(a_vector.x, a_newY);
    }

    // Color Extensions
    public static string ToHexStringRGBA(this Color a_color)
    {
        string rs = a_color.r.ToString("X");
        string gs = a_color.g.ToString("X");
        string bs = a_color.b.ToString("X");
        string a_s = a_color.a.ToString("X");
        while (rs.Length < 2) rs = "0" + rs;
        while (gs.Length < 2) gs = "0" + gs;
        while (bs.Length < 2) bs = "0" + bs;
        while (a_s.Length < 2) a_s = "0" + a_s;
        return rs + gs + bs + a_s;
    }
}
