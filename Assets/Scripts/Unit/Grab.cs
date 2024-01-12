using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grab : UnitObject, IGrab
{
    public enum GrabState { 
        TryGrab,
        Grabing,
        TryPutDown
    }

    // 机械臂状态
    // 尝试抓取 --(成功抓取)--> -> 旋转 -> 尝试放下 --(如果成功放下)--> 转回来(50坐地能吸土直接靠吸的) -> 尝试抓取 
    [SerializeField] private Item item;
    [SerializeField] private GrabState state;

    private void Start()
    {
        state = GrabState.TryGrab;
    }

    private void Update()
    {
        //简单状态机 之后可以改成状态模式
        switch (state)
        {
            case GrabState.TryGrab:
                if (TryGrab())
                {
                    state = GrabState.Grabing;
                    item.transform.DOLocalMove(new Vector3(0, 0.5f), 0.3f).OnComplete(() => { this.state = GrabState.TryGrab; });
                }
                break;
            case GrabState.Grabing:
                break;
            case GrabState.TryPutDown:
                // TODO : 放下
                break;
            default:
                break;
        }
    }

    public bool TryGrab()
    {
        if (item != null)
        {
            return true;
        }

        UnitObject downSideUnit = GetUnitObjectOnGrid(position + Vector2Int.down.VecterRotateByDir(dir));

        if (downSideUnit == null)
        {
            return false;
        }

        // 如果这一个格子是可抓取的地方
        if (downSideUnit is IBeGrabItem)
        {
            IBeGrabItem downBeGrab = downSideUnit as IBeGrabItem;
            if (downBeGrab.TryGrabItem(out Item item))
            {
                this.item = item;
                item.transform.SetParent(transform);
                if (Console.Instance.Active) LogUtilsXY.LogOnPos("抓取到物品", transform.position);
            }
        }

        return false;
    }
}
