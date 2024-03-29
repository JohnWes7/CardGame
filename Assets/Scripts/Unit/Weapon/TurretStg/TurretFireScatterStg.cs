using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireScatterStg : TurretFireStrategyBase
{
    [SerializeField] private float scatterAngle = 60f;
    [SerializeField] private int scatterCount = 3;


    public override void FireStg(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO)
    {
        Vector2 direction = CalculateDir(turretWeaponV2);
        float a = scatterAngle / (scatterCount - 1);

        // 以direction为中心，scatterAngle为角度，scatterCount为数量，生成散射方向
        for (int i = 0; i < scatterCount; i++)
        {
            float angle = a * (i - (scatterCount - 1) / 2f);
            Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * direction;

            // 生成子弹
            Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
                projectileSO,
                turretWeaponV2.Target,
                turretWeaponV2.ProjectileCreatPos.position,
                scatterDirection);
            Projectile.ProjectileCreateFactory(projectileCreationParams);
        }

     
    }

    [ContextMenu("DEBUG_TEST")]
    public void DEBUG_TEST()
    {
        Vector2 direction = Vector2.up;
        float a = scatterAngle / (scatterCount - 1);

        // 以direction为中心，scatterAngle为角度，scatterCount为数量，生成散射方向
        for (int i = 0; i < scatterCount; i++)
        {
            float angle = a * (i - (scatterCount - 1) / 2f);
            Vector2 scatterDirection = Quaternion.Euler(0, 0, angle) * direction;
            Debug.Log(scatterDirection);
        }
    }
}
