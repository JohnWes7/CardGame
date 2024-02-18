using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DebugTool
{
    [MenuItem("YLuoDebugTool/LogInventory")]
    public static void LogInventory()
    {
        Debug.Log(PlayerModel.Instance.GetInventory().ToString());
    }

    [MenuItem("YLuoDebugTool/LogPath")]
    public static void LogPath()
    {
        Debug.Log($"Application.dataPath: {Application.dataPath}");
        Debug.Log($"Application.persistentDataPath: {Application.persistentDataPath}");
        Debug.Log($"Application.streamingAssetsPath: {Application.streamingAssetsPath}");
    }

    [MenuItem("YLuoDebugTool/LogUnitInfoModel")]
    public static void LogUnitInfoModel()
    {
        UnitInfoModel.Instance.Refresh();
        Debug.Log(UnitInfoModel.Instance.ToString());
    }
}
