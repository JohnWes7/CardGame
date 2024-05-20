using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class TurretFireRandomRangeStg : TurretFireStrategyBase, ITurretFireStg
{
    [SerializeField] private float randomRange;

    //public override void FireStg(AbstractTurret turretWeaponV2, ProjectileSO projectileSO)
    //{
    //    Vector2 direction = CalculateDir(turretWeaponV2);

    //    // 以direction为中心，randomRange为角度，生成一个随机方向
    //    float angle = Random.Range(-randomRange, randomRange);
    //    Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * direction;

    //    // 生成子弹
    //    Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
    //                   projectileSO,
    //                   turretWeaponV2.Target,
    //                   turretWeaponV2.ProjectileCreatePos.position,
    //                   scatterDirection);
    //    Projectile.ProjectileCreateFactory(projectileCreationParams);
    //}

    public override void FireStg(AbstractTurret turretWeaponV2, ProjectileSO projectileSO)
    {
        throw new System.NotImplementedException();
    }
}
