using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class RelicModel : AbstractModel
{
    // 所有的遗物
    public List<RelicSO> allRelics;

    protected override void OnInit()
    {
        Refresh();
    }

    public void Refresh()
    {
        // 从Resources文件夹中加载所有的遗物
        RelicSO[] relicSOs = Resources.LoadAll<RelicSO>("Default/Relic");
        allRelics = new List<RelicSO>(relicSOs);

        Debug.Log("RelicModel: init relic list:" + Johnwest.JWUniversalTool.ListToString(allRelics));
    }

    public T GetRelicSO<T> (string name) where T : RelicSO
    {
        foreach (var item in allRelics)
        {
            if (item.name == name && item is T)
            {
                return item as T;
            }
        }
        return null;
    }

    public List<T> GetAllRelic<T>() where T : RelicSO
    {
        List<T> result = new List<T>();
        foreach (RelicSO item in allRelics)
        {
            if (item is T itemT)
            {
                result.Add(itemT);
            }
        }
        return result;
    }
}
