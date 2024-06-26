using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

/// <summary>
/// 固定激光子弹
/// 注意 激光类子弹的target需要自己计算或者从turret处更新
/// </summary>
public class FixedLaserProjectile : LaserProjectileBase
{
    public override void DoDamage(LaserUpdateParams laserUpdateParams)
    {
        // 通过射线检测调整距离
        RaycastHit2D hit = Physics2D.Raycast(
            laserUpdateParams.turret.GetProjectileCreatePos(),
            laserUpdateParams.turret.GetFireDir(),
            laserUpdateParams.turret.TurretSO.radius,
            projectileSO.targetLayer
        );
        
        //Debug.Log("计算伤害");
        if (hit.collider != null && hit.collider.TryGetComponent<IBeDamage>(out IBeDamage beDamage))
        {
            beDamage.BeDamage(new DamageInfo(projectileSO.damage, creator));
        }
    }

    public override void LaserUpdate(LaserUpdateParams laserUpdateParams)
    {
        // 通过射线检测调整距离
        RaycastHit2D hit = Physics2D.Raycast(
            laserUpdateParams.turret.GetProjectileCreatePos(),
            laserUpdateParams.turret.GetFireDir(),
            laserUpdateParams.turret.TurretSO.radius,
            projectileSO.targetLayer
        );

        // 如果有碰撞则设置碰撞点
        if (hit.collider != null)
        {
            target = hit.collider.transform;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, laserUpdateParams.turret.GetProjectileCreatePos());
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            target = null;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, laserUpdateParams.turret.GetProjectileCreatePos());
            lineRenderer.SetPosition(1, laserUpdateParams.turret.GetProjectileCreatePos() + (Vector3)(laserUpdateParams.turret.GetFireDir() * laserUpdateParams.turret.TurretSO.radius));
        }
    }
}