using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class UnitInfoModel : Singleton<UnitInfoModel>
{
    private Dictionary<string, UnitSO> allUnitDict;

    public UnitInfoModel()
    {
        Refresh();
    }

    public void Refresh()
    {
        allUnitDict = new Dictionary<string, UnitSO>();

        var allUnit = Resources.LoadAll<UnitSO>("Default/Unit/UnitSO");

        foreach (var item in allUnit)
        {
            allUnitDict[item.name] = item;
        }
    }

    public override string ToString()
    {
        List<string> nameList = new List<string>();
        foreach (var item in allUnitDict.Keys)
        {
            nameList.Add(item);
        }
        return $"num: {allUnitDict.Count}\n" + string.Join(", ", nameList);
    }

    public UnitSO GetUnit(string unitName)
    {
        if (allUnitDict.TryGetValue(unitName, out UnitSO unitSO))
        {
            return unitSO;
        }

        Johnwest.JWUniversalTool.LogWarningWithClassMethodName($"no this unit, name: {unitName}", System.Reflection.MethodBase.GetCurrentMethod());
        return null;
    }
}
