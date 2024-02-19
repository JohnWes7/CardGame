using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoModel : Singleton<ItemInfoModel>
{
    private Dictionary<string, ItemSO> allItemDict;

    public ItemInfoModel()
    {
        Refresh();
    }

    public void Refresh()
    {
        allItemDict = new Dictionary<string, ItemSO>();

        var allItem = Resources.LoadAll<ItemSO>("Default/Item/ItemData");

        foreach (var item in allItem)
        {
            allItemDict[item.name] = item;
        }
    }

    public override string ToString()
    {
        List<string> nameList = new List<string>();
        foreach (var item in allItemDict.Keys)
        {
            nameList.Add(item);
        }
        return $"num: {allItemDict.Count}\n" + string.Join(", ", nameList);
    }

    public ItemSO GetItem(string itemName)
    {
        if (allItemDict.TryGetValue(itemName, out ItemSO ItemSO))
        {
            return ItemSO;
        }

        Johnwest.JWUniversalTool.LogWarningWithClassMethodName($"no this item, name: {itemName}", System.Reflection.MethodBase.GetCurrentMethod());
        return null;
    }
}
