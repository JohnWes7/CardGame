using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

[System.Serializable]
public class ShipMemento
{
    [System.Serializable]
    public class MementoUnitInfo {
        public string unitName;
        public Dir dir;
        public int x;
        public int y;

        // extra data 额外数据
        public Dictionary<string, object> extraData;

        public MementoUnitInfo(string unitName, Dir dir, Vector2Int gridPos, Dictionary<string, object> extraData = null)
        {
            if (!(extraData is null))
            {
                this.extraData = extraData;
            }

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

        List<UnitObject> unitList = new List<UnitObject>();
        foreach (List<FGridNode> item in shipController.Grid.Content)
        {
            foreach (FGridNode gridobj in item)
            {
                UnitObject unitObject = gridobj != null ? gridobj.GetContent() : null;

                if (unitObject != null && !unitList.Contains(unitObject))
                {
                    unitList.Add(unitObject);
                    Debug.Log(unitObject);
                }
            }
        }

        mementoUnitInfoList = new List<MementoUnitInfo>();

        foreach (UnitObject unitObject in unitList)
        {
            if (unitObject is IExtraUnitObjectData extraUnitObject)
            {
                Dictionary<string, object> extraData = extraUnitObject.GetExtraData();
                // 现在你可以使用 extraData 来构造 MementoUnitInfo
                mementoUnitInfoList.Add(new MementoUnitInfo(unitObject.UnitSO.name, unitObject.Dir, unitObject.Position, extraData));
            }
            else
            {
                // 对于没有额外数据的单位，继续使用现有的构造方式
                mementoUnitInfoList.Add(new MementoUnitInfo(unitObject.UnitSO.name, unitObject.Dir, unitObject.Position));
            }

        }
    }
}

/// <summary>
/// 如果有的unit有额外数据 继承这个接口
/// </summary>
public interface IExtraUnitObjectData
{
    public Dictionary<string, object> GetExtraData();
    public void SetExtraData(Dictionary<string, object> data);
}
