using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class TurretFireRoundBurstStg : TurretFireStrategyBase, ITurretFireStg
{
    //[SerializeField] private int burstCount = 3;
    [SerializeField] private float burstDelay = 0.1f;

    public override void FireStg(AbstractTurret abstractTurret, ProjectileSO projectileSO)
    {
        StartCoroutine(FireStgCoroutine(abstractTurret, projectileSO, burstDelay));
    }

    public IEnumerator FireStgCoroutine(AbstractTurret abstractTurret, ProjectileSO projectileSO, float delay)
    {

        //for (int i = 0; i < burstCount; i++)
        //{
        //    Projectile.ProjectileCreationParams creationParams = new Projectile.ProjectileCreationParams(
        //        projectileSO,
        //        abstractTurret.GetTarget(),
        //        abstractTurret.GetTarget().position,
        //        abstractTurret.GetFireDir(),
        //        abstractTurret
        //        );
        //    Projectile.ProjectileCreateFactory(creationParams);
        yield return new WaitForSeconds(delay);
        //}
    }
}
