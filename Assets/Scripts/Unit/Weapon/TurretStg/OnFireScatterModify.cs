using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 增加子弹并且散射
/// </summary>
public class OnFireScatterModify : OnFireModifyBase
{
    public float scatterAngle = 60f;
    public int scatterCount = 3;

    protected override void OnFireModify(object sender, FireEventArgs e)
    {
        float a = scatterAngle / (scatterCount - 1);

        e.ProjectileCreationParams.Clear();
        // 以direction为中心，scatterAngle为角度，scatterCount为数量，生成散射方向
        for (int i = 0; i < scatterCount; i++)
        {
            float angle = a * (i - (scatterCount - 1) / 2f);
            Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * e.Direction;

            // 生成子弹参数
            Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
                e.ProjectileSO,
                e.Target,
                e.CreatePosition,
                scatterDirection,
                e.creator);

            // 加入到参数列表等待生成
            e.ProjectileCreationParams.Add(projectileCreationParams);
        }
    }
}
