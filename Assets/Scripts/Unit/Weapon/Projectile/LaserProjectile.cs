using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public interface ILaserProjectile
{
    void LaserUpdate(AbstractTurret turret, float deltaTime);
}

public abstract class LaserProjectile : Projectile, ILaserProjectile
{
    [SerializeField, ForceFill] private LineRenderer lineRenderer;



    public void DoDamage()
    {

    }

    public void LaserUpdate(AbstractTurret turret, float deltaTime)
    {
        throw new NotImplementedException();
    }
}