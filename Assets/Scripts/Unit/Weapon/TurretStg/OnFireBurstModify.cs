using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 修改一次发射多个子弹
/// </summary>
public class OnFireBurstModify : OnFireModifyBase
{
    public int burstCount = 3;
    public float burstDelay = 0.1f;

    /// <summary>
    /// 覆写设计次数和间隔时间
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected override void OnFireModify(object sender, FireEventArgs e)
    {
        e.burstCount = burstCount;
        e.burstDelay = burstDelay;
    }
}
