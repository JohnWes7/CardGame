using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public interface ILaserProjectile
{
    void LaserUpdate(AbstractTurret turret, float deltaTime);
    public void OpenLaser(AbstractTurret turret);
    public void CloseLaser(AbstractTurret turret);
}

public abstract class LaserProjectileBase : Projectile, ILaserProjectile
{
    [SerializeField, ForceFill] protected LineRenderer lineRenderer;

    public virtual void DoDamage(AbstractTurret abstractTurret)
    {
        //Debug.Log("计算伤害");
        if (target.TryGetComponent<IBeDamage>(out IBeDamage beDamage))
        {
            beDamage.BeDamage(new DamageInfo(projectileSO.damage, abstractTurret));
        }
    }

    public virtual void LaserUpdate(AbstractTurret turret, float deltaTime)
    {
        // 之后可以把这个包装为一个 command 便于管理
        target = turret.GetTarget();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, turret.GetProjectileCreatePos());
        lineRenderer.SetPosition(1, target.transform.position);
    }

    public virtual void OpenLaser(AbstractTurret turret)
    {
        gameObject.SetActive(true);
    }

    public virtual void CloseLaser(AbstractTurret turret)
    {
        gameObject.SetActive(false);
    }
}


