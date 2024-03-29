using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireRandomDirModify : MonoBehaviour
{
    public float randomAngle = 60f;
    public int provity = 5;

    private void OnEnable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.AddListener(OnFireRandomDirModify_OnFire, provity);
    }

    private void OnDisable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.RemoveListener(OnFireRandomDirModify_OnFire);
    }

    /// <summary>
    /// 遍历所有子弹参数 并在原有的方向上随机修改方向
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnFireRandomDirModify_OnFire(object sender, FireEventArgs e)
    {
        foreach (Projectile.ProjectileCreationParams para in e.ProjectileCreationParams)
        {
            para.direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-randomAngle / 2, randomAngle / 2)) * para.direction;
        }
    }
}
