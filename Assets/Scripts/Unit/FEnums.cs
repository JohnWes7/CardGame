using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    up,
    right,
    down,
    left
}

public static class DirExtensions
{
    /// <summary>
    /// 获得下一个方向
    /// </summary>
    /// <param name="curDir"></param>
    /// <returns></returns>
    public static Dir GetNextDir(Dir curDir)
    {
        if (curDir == Dir.left)
        {
            return Dir.up;
        }
        else
        {
            return ++curDir;
        }
    }

    /// <summary>
    /// 用dir获取需要转多少度
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Quaternion DirToQuaternion(Dir dir)
    {
        switch (dir)
        {
            case Dir.up:
                return Quaternion.identity;
            case Dir.right:
                return Quaternion.Euler(0, 0, -90f);
            case Dir.down:
                return Quaternion.Euler(0, 0, 180f);
            case Dir.left:
                return Quaternion.Euler(0, 0, 90f);
            default:
                return Quaternion.identity;
        }
    }
}
