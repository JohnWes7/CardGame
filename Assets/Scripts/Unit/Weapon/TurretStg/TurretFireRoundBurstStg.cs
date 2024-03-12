using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireRoundBurstStg : TurretFireStrategyBase, ITurretFireStg
{
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstDelay = 0.1f;

    public override void FireStg(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO)
    {
        StartCoroutine(FireStgCoroutine(turretWeaponV2, projectileSO, burstDelay));
    }

    public IEnumerator FireStgCoroutine(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO, float delay)
    {
        Vector2 direction = CalculateDir(turretWeaponV2);

        for (int i = 0; i < burstCount; i++)
        {
            if (turretWeaponV2.Target != null)
            {
                direction = CalculateDir(turretWeaponV2);
            }

            Projectile.ProjectileCreationParams creationParams = new Projectile.ProjectileCreationParams(
                projectileSO,
                turretWeaponV2.Target,
                turretWeaponV2.ProjectileCreatPos.position,
                direction,
                turretWeaponV2
                );
            Projectile.ProjectileCreateFactory(creationParams);
            yield return new WaitForSeconds(delay);
        }
    }
}
