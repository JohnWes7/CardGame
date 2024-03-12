using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireScatterModify : MonoBehaviour
{
    public float scatterAngle = 60f;
    public int scatterCount = 3;
    public int priority = 5;

    private void OnEnable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.AddListener(OnFireScatterModify_OnFire, priority);
    }

    private void OnDisable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.RemoveListener(OnFireScatterModify_OnFire);
    }

    private void OnFireScatterModify_OnFire(object sender, FireEventArgs e)
    {
        Vector2 direction = e.Target.position - e.CreatePosition;
        float a = scatterAngle / (scatterCount - 1);

        e.ProjectileCreationParams.Clear();
        // 以direction为中心，scatterAngle为角度，scatterCount为数量，生成散射方向
        for (int i = 0; i < scatterCount; i++)
        {
            float angle = a * (i - (scatterCount - 1) / 2f);
            Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * direction;

            // 生成子弹参数
            Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
                e.ProjectileSO,
                e.Target,
                e.CreatePosition,
                scatterDirection);

            // 加入到参数列表等待生成
            e.ProjectileCreationParams.Add(projectileCreationParams);
        }
    }
}
