using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 散射方向修改
/// </summary>
public class OnFireScatterDirModify : OnFireModifyBase
{
    public float scatterAngle = 60f;

    protected override void OnFireModify(object sender, FireEventArgs e)
    {
        float a = scatterAngle / (e.ProjectileCreationParams.Count - 1);

        for (int i = 0; i < e.ProjectileCreationParams.Count; i++)
        {
            float angle = a * (i - (e.ProjectileCreationParams.Count - 1) / 2f);
            Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * e.Direction;

            e.ProjectileCreationParams[i].direction = scatterDirection;
        }
    }
}
