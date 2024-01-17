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
                    // 如果成功抓取则切换状态并且开始执行移动动画
                    state = GrabState.Grabing;
                    // 移动动画结束开始tryputdown
                    item.transform.DOLocalMove(new Vector3(0, 0.5f), 0.3f).OnComplete(() => { this.state = GrabState.TryPutDown; });
                }
                break;
            case GrabState.Grabing:
                break;
            case GrabState.TryPutDown:
                if (TryPutDown())
                {
                    // 如果成功放下改为抓取状态
                    state = GrabState.TryGrab;
                }
                break;
            default:
                break;
        }
    }

    public bool TryPutDown()
    {
        // 错误 没有物品当作已经放下了
        if (item == null)
        {
            return true;
        }

        Vector2Int downPos = position + Vector2Int.up.VecterRotateByDir(dir);
        var upsideUnit = GetUnitObjectOnGrid(downPos);
        //Debug.Log(downPos);

        if (upsideUnit == null)
        {
            return false;
        }

        if (upsideUnit is IBePutDownGrabItem)
        {
            var upsidePutDownUnit = upsideUnit as IBePutDownGrabItem;
            //Debug.Log("尝试放下");
            if (upsidePutDownUnit.TryPutDownItem(this.item))
            {
                item = null;
                if (Console.Instance.Active) LogUtilsXY.LogOnPos("放下物品", transform.position);
                return true;
            }
        }

        return false;
    }

    public bool TryGrab()
    {
        // 错误 如果已经有了物品就当作抓取到了 防止方法触发两次
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

            // 抓取的目的地
            UnitObject upsideUnit = GetUnitObjectOnGrid(position + Vector2Int.up.VecterRotateByDir(dir));
            IBePutDownGrabItem upsidBePutDownGrabItem = (upsideUnit != null && upsideUnit is IBePutDownGrabItem) ? upsideUnit as IBePutDownGrabItem : null;

            // 要抓取的物品一定要是需要的才行
            if (IsItemInNeed(downBeGrab, upsidBePutDownGrabItem))
            {
                if (downBeGrab.TryGrabItem(out Item item))
                {
                    this.item = item;
                    item.transform.SetParent(transform);
                    if (Console.Instance.Active) LogUtilsXY.LogOnPos("抓取到物品", transform.position);
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 如果bePutDown没有物体或者beputdown返回的inneed是null 不管能抓到什么物体都算是需要的物品
    /// </summary>
    /// <param name="beGrabItem"></param>
    /// <param name="bePutDownGrab"></param>
    /// <returns></returns>
    private bool IsItemInNeed(IBeGrabItem beGrabItem, IBePutDownGrabItem bePutDownGrab)
    {
        var needList = bePutDownGrab?.ItemSOInNeed();
        if (needList == null)
        {
            return true;
        }

        return needList.Contains(beGrabItem.Peek());
    }
}
