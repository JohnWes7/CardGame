using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFireMultiProjectileModify : MonoBehaviour
{
    public int targetCount = 3;
    public int provity = 1;

    private void OnEnable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.AddListener(OnFireMultiProjectileModify_OnFire, provity);
    }

    public void OnDisable()
    {
        IOnFire onFire = GetComponent<IOnFire>();
        onFire.priorityEventManager.RemoveListener(OnFireMultiProjectileModify_OnFire);
    }

    private void OnFireMultiProjectileModify_OnFire(object sender, FireEventArgs e)
    {
        if (e.ProjectileCreationParams.Count >= targetCount)
        {
            return;
        }

        Vector2 dir = e.Target.position - e.CreatePosition;

        
        // 如果子弹数量小于targetCount，就生成targetCount - e.ProjectileCreationParams.Count个子弹
        int diff = targetCount - e.ProjectileCreationParams.Count;
        for (int i = 0; i < diff; i++)
        {
            // 生成子弹参数
            Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
                            e.ProjectileSO,
                            e.Target,
                            e.CreatePosition,
                            dir);

            // 加入到参数列表等待生成
            e.ProjectileCreationParams.Add(projectileCreationParams);
        }
    }
}
