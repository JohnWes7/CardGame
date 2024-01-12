using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSimple : UnitObject, IShipUnit, IBelt, IBeGrabItem
{

    [SerializeField] private Item item;
    [SerializeField] private MonoInterface<IShipController> ship;
    [SerializeField] private bool pauseOneTick;

    public bool CanInsertItem()
    {
        return item == null;
    }

    public bool EnqueueItem(Item enqueueItem)
    {
        if (this.item == null)
        {
            this.item = enqueueItem;

            // 设置位置
            enqueueItem.transform.SetParent(transform);
            enqueueItem.transform.localPosition = Vector3.zero;
            if (Console.Instance.Active) LogUtilsXY.LogOnPos("添加成功", transform.position);
            pauseOneTick = true;

            return true;
        }

        if (Console.Instance.Active) LogUtilsXY.LogOnPos("无法添加", transform.position);
        return false;
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public int MaxRestLenghtFrom0Index()
    {
        if (item == null)
        {
            return 10;
        }

        return 0;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public bool TryInsertItem(Item insertItem)
    {
        return EnqueueItem(insertItem);
    }

    private void OnEnable()
    {
        Tick.Ontick += OnTick;
        pauseOneTick = false;
    }

    private void OnDisable()
    {
        Tick.Ontick -= OnTick;
    }


    public void OnTick(object sender, Tick.OnTickEventArgs args)
    {
        if (item == null)
        {
            return;
        }

        if (pauseOneTick)
        {
            pauseOneTick = false;
            return;
        }

        var nextGridObj = grid?.GetGridObject(position + Vector2Int.up.VecterRotateByDir(dir));
        var nextUnit = nextGridObj == null ? null : nextGridObj.GetContent();
        if (nextUnit == null)
        {
            //if (Console.Instance.Active) LogUtilsXY.LogOnPos("没有下一个传送带", transform.position);
            return;
        }

        if (nextUnit is IBelt)
        {
            var nextBelt = nextUnit as IBelt;
            if (nextBelt.EnqueueItem(item))
            {
                if (Console.Instance.Active) LogUtilsXY.LogOnPos("移交给下一个传送带", transform.position);
                item = null;
            }
        }
    }

    public bool TryGrabItem(out Item item)
    {
        if (this.item != null)
        {
            item = this.item;
            this.item = null;
            return true;
        }

        item = null;
        return false;
    }
}
