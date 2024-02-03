using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public class ShipMemento
{
    [System.Serializable]
    public class MementoUnitInfo {
        public string unitName;
        public Dir dir;
        public int x;
        public int y;

        public MementoUnitInfo(string unitName, Dir dir, Vector2Int gridPos)
        {
            this.unitName = unitName;
            this.dir = dir;
            x = gridPos.x;
            y = gridPos.y;
        }
    }

    // 存储飞船grid的大小的数据
    public int gridWidthSize;
    public int gridHeightSize;

    // 存储所有的unit重新生成所需要的数据
    public List<MementoUnitInfo> mementoUnitInfoList;

    public ShipMemento(IShipController shipController)
    {
        if (shipController == null)
        {
            return;
        }

        gridWidthSize = shipController.Grid.GetWidth();
        gridHeightSize = shipController.Grid.GetHeight();

        Dictionary<UnitObject, MementoUnitInfo> unitList = new Dictionary<UnitObject, MementoUnitInfo>();
        foreach (var item in shipController.Grid.Content)
        {
            foreach (var gridobj in item)
            {
                UnitObject unitObject = gridobj != null ? gridobj.GetContent() : null;
                if (unitObject != null && !unitList.ContainsKey(unitObject))
                {
                    unitList.Add(unitObject, new MementoUnitInfo(unitObject.UnitSO.name, unitObject.Dir, unitObject.Position));
                }
            }
        }

        mementoUnitInfoList ??= new List<MementoUnitInfo>();
        mementoUnitInfoList.Clear();

        foreach (var item in unitList)
        {
            mementoUnitInfoList.Add(item.Value);
        }
    }

    /// <summary>
    /// 将纪念品保存到本地
    /// </summary>
    public void SaveMemento(string filePath)
    {
        string json = JsonConvert.SerializeObject(this);
        try
        {
            File.WriteAllText(filePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("写入失败\n" + e.Message);
        }

        Debug.Log("保存成功");
    }
}
