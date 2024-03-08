using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireNormalStg : MonoBehaviour, ITurretFireStg
{
    /// <summary>
    /// 开火策略模式
    /// 普通开火 生成一次子弹射向目标
    /// </summary>
    public void FireStg(TurretWeaponV2 turretWeapon, ProjectileSO projectileSO)
    {
        // 生成子弹
        Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
            projectileSO, 
            turretWeapon.Target, 
            turretWeapon.ProjectileCreatPos.position, 
            turretWeapon);
        Projectile.ProjectileCreateFactory(projectileCreationParams);
    }
}

public interface ITurretFireStg
{
    public void FireStg(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO);
}
