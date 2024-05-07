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
        LoadInResourcePath("Default/Unit/UnitSO");
        LoadInResourcePath("Default/Weapon/WeaponUnitSO");

        void LoadInResourcePath(string path)
        {
            var allUnit = Resources.LoadAll<UnitSO>(path);

            foreach (var item in allUnit)
            {
                allUnitDict[item.name] = item;
            }
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

        Debug.LogWarning($"no this unit, name: {unitName}");
        return null;
    }
}
