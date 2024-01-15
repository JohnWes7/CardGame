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
}

public interface IBePutDownGrabItem
{
    /// <summary>
    /// 能放下Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool TryPutDownItem(Item item);
}

