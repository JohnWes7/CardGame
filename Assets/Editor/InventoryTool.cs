using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InventoryTool
{
    [MenuItem("tool/LogInventory")]
    public static void LogInventory()
    {
        Debug.Log(PlayerInventory.Instance.ToString());
    }
}
