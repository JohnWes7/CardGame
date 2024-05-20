using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 增加子弹数量
/// </summary>
public class OnFireMultiProjectileModify : OnFireModifyBase
{
    public int targetCount = 3;

    protected override void OnFireModify(object sender, FireEventArgs e)
    {
        if (e.ProjectileCreationParams.Count >= targetCount)
        {
            return;
        }

        // 如果子弹数量小于targetCount，就生成targetCount - e.ProjectileCreationParams.Count个子弹
        int diff = targetCount - e.ProjectileCreationParams.Count;
        for (int i = 0; i < diff; i++)
        {
            // 生成子弹参数
            Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
                e.ProjectileSO,
                e.Target,
                e.CreatePosition,
                e.Direction,
                e.creator);

            // 加入到参数列表等待生成
            e.ProjectileCreationParams.Add(projectileCreationParams);
        }
    }
}
