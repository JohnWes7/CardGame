using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class LaserUpdateParams
{
    public AbstractTurret turret;
    public Vector2 dir;
    public float deltaTime;
}

public interface ILaserProjectile
{
    void LaserUpdate(LaserUpdateParams laserUpdateParams);
    public void OpenLaser(LaserUpdateParams laserUpdateParams);
    public void CloseLaser(LaserUpdateParams laserUpdateParams);
    public void DoDamage(LaserUpdateParams laserUpdateParams);
}

public abstract class LaserProjectileBase : Projectile, ILaserProjectile
{
    [SerializeField, ForceFill] protected LineRenderer lineRenderer;

    public virtual void DoDamage(LaserUpdateParams laserUpdateParams)
    {
        //Debug.Log("计算伤害");
        if (target.TryGetComponent<IBeDamage>(out IBeDamage beDamage))
        {
            beDamage.BeDamage(new DamageInfo(projectileSO.damage, laserUpdateParams.turret));
        }
    }

    public virtual void LaserUpdate(LaserUpdateParams laserUpdateParams)
    {
        // 之后可以把这个包装为一个 command 便于管理
        target = laserUpdateParams.turret.GetTarget();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, laserUpdateParams.turret.GetProjectileCreatePos());
        lineRenderer.SetPosition(1, target.transform.position);
    }

    public virtual void OpenLaser(LaserUpdateParams laserUpdateParams)
    {
        gameObject.SetActive(true);
    }

    public virtual void CloseLaser(LaserUpdateParams laserUpdateParams)
    {
        gameObject.SetActive(false);
    }
}


