using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool
{
    public static Vector2 VecterRotateByDir(this Vector2 vector2, Dir dir)
    {
        return dir switch
        {
            Dir.up => vector2,
            Dir.right => new Vector2(vector2.y, -vector2.x),
            Dir.down => -vector2,
            Dir.left => new Vector2(-vector2.y, vector2.x),
            _ => vector2,
        };
    }

    public static Vector3 VecterRotateByDir(this Vector3 vector3, Dir dir)
    {
        return dir switch
        {
            Dir.up => vector3,
            Dir.right => new Vector3(vector3.y, -vector3.x),
            Dir.down => -vector3,
            Dir.left => new Vector3(-vector3.y, vector3.x),
            _ => vector3,
        };
    }

    public static Vector2Int VecterRotateByDir(this Vector2Int vector2Int, Dir dir)
    {
        return dir switch
        {
            Dir.up => vector2Int,
            Dir.right => new Vector2Int(vector2Int.y, -vector2Int.x),
            Dir.down => -vector2Int,
            Dir.left => new Vector2Int(-vector2Int.y, vector2Int.x),
            _ => vector2Int,
        };
    }
}
