using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Belt : UnitObject, IShipUnit, IBelt
{

    [SerializeField] private MonoInterface<IShipController> ship;
    [SerializeField] private BeltPoint[] points;

    //scriptObject
    [SerializeField] private int beltSpeed;

    [Serializable]
    public struct BeltPoint {
        public Vector3 point;
        public Item item;
    }

    public bool TryInsertItem(Item item)
    {
        if (CanInsertItem())
        {
            item.transform.SetParent(transform);
            int index = points.Length / 2 - 1;
            
            points[index].item = item;
            item.transform.localPosition = points[index].point.VecterRotateByDir(dir);

            return true;
        }

        return false;
    }

    public bool CanInsertItem()
    {
        bool condition = true;
        for (int i = points.Length/2 - 3; i < points.Length/2 + 2; i++)
        {
            if (points[i].item != null)
            {
                LogUtilsXY.LogOnPos("不能插入", transform.position - Vector3.forward);
                condition = false;
                break;
            }
        }

        return condition;
    }


    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    // 之后可能需要改成tick
    private void FixedUpdate()
    {
        // 判断下一个移动的最大位置
        // 即能移动的最大距离

        // 先判断下一个是不是传送带
        Vector2Int nextBeltPos = position + Vector2Int.up.VecterRotateByDir(dir);
        UnitObject unit = grid.GetGridObject(nextBeltPos)?.GetContent();

        if (unit is IBelt)
        {

        }
        else
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].item != null)
                {
                    int p = i + 5;
                    
                }
            }
        }
    }

}
