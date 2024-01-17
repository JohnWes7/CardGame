using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeGrabItem
{
    /// <summary>
    /// 能从中抓取物品
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryGrabItem(out Item item);
    public ItemSO Peek();
}

public interface IBePutDownGrabItem
{
    /// <summary>
    /// 能放下Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryPutDownItem(Item item);

    /// <summary>
    /// 标记需要什么类型的物品
    /// </summary>
    /// <returns>如果 return null 说明可以要任何东西</returns>
    public List<ItemSO> ItemSOInNeed()
    {
        return null;
    }
}

