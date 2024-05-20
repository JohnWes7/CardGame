using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireRandomDirModify : OnFireModifyBase
{
    public float randomAngle = 60f;

    /// <summary>
    /// 遍历所有子弹参数 并在原有的方向上随机修改方向
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnFireModify(object sender, FireEventArgs e)
    {
        foreach (Projectile.ProjectileCreationParams para in e.ProjectileCreationParams)
        {
            para.direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-randomAngle / 2, randomAngle / 2)) * para.direction;
        }
    }
}
