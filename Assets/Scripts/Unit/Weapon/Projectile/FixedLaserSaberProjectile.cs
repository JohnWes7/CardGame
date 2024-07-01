using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// 固定激光子弹
/// 注意 激光类子弹的target需要自己计算或者从turret处更新
/// </summary>
public class FixedLaserSaberProjectile : LaserProjectileBase
{
    public override void DoDamage(LaserUpdateParams laserUpdateParams)
    {
        // 通过射线检测调整距离
        var hit = Physics2D.RaycastAll(
            laserUpdateParams.turret.GetProjectileCreatePos(),
            laserUpdateParams.turret.GetFireDir(),
            laserUpdateParams.turret.TurretSO.radius,
            projectileSO.targetLayer
        );

        foreach (var item in hit)
        {
            // 获取Ibedamage
            if (item.collider != null)
            {
                if (item.collider.TryGetComponent<IBeDamage>(out IBeDamage bedamage))
                {
                    bedamage.BeDamage(new DamageInfo(projectileSO.damage, laserUpdateParams.turret));
                }
            }
        }
    }

    public override void LaserUpdate(LaserUpdateParams laserUpdateParams)
    {
        // 画一个直接到炮塔最远端的线
        target = null;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, laserUpdateParams.turret.GetProjectileCreatePos());
        lineRenderer.SetPosition(1, laserUpdateParams.turret.GetProjectileCreatePos()
            + (Vector3)(laserUpdateParams.turret.GetFireDir() * laserUpdateParams.turret.TurretSO.radius));
    }
}