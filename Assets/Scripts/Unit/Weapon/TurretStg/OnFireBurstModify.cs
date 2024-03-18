using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireBurstModify : MonoBehaviour
{
    public int provity = 6;
    public int burstCount = 3;
    public float burstDelay = 0.1f;

    private void OnEnable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.AddListener(OnFireBurstModify_OnFire, provity);
    }

    private void OnDisable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.RemoveListener(OnFireBurstModify_OnFire);
    }

    /// <summary>
    /// 覆写设计次数和间隔时间
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnFireBurstModify_OnFire(object sender, FireEventArgs e)
    {
        e.burstCount = burstCount;
        e.burstDelay = burstDelay;
    }
}
