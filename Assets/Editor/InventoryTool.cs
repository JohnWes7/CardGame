using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DebugTool
{
    [MenuItem("Johnwest/LogInventory")]
    public static void LogInventory()
    {
        Debug.Log(PlayerModel.Instance.GetInventory().ToString());
    }

    [MenuItem("Johnwest/LogPath")]
    public static void LogPath()
    {
        Debug.Log($"Application.dataPath: {Application.dataPath}");
        Debug.Log($"Application.persistentDataPath: {Application.persistentDataPath}");
        Debug.Log($"Application.streamingAssetsPath: {Application.streamingAssetsPath}");

    }

    [MenuItem("Johnwest/LogSpriteAniPrefabName")]
    public static void LogSpriteAniPrefabName()
    {
        Johnwest.LogSprirtAni.Instance.GetPrefabName();
    }


    [MenuItem("Johnwest/LogUnitInfoModel")]
    public static void LogUnitInfoModel()
    {
        UnitInfoModel.Instance.Refresh();
        Debug.Log(UnitInfoModel.Instance.ToString());
    }

    [MenuItem("Johnwest/LogItemInfoModel")]
    public static void LogItemInfoModel()
    {
        ItemInfoModel.Instance.Refresh();
        Debug.Log(ItemInfoModel.Instance.ToString());
    }

    [MenuItem("Johnwest/TestAutoStackTrace")]
    public static void TestAutoStackTrace()
    {
        // 获取当前方法的方法信息
        System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();

        // 获取类名和方法名
        string className = method.ReflectedType.Name;
        string methodName = method.Name;

        // 输出带有类名和方法名的日志
        Debug.Log($"{className}.{methodName}: test");
    }
}
