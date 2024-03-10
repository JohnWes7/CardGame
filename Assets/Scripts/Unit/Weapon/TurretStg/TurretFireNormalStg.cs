using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireNormalStg : TurretFireStrategyBase, ITurretFireStg
{
    /// <summary>
    /// 开火策略模式
    /// 普通开火 生成一次子弹射向目标
    /// </summary>
    public override void FireStg(TurretWeaponV2 turretWeapon, ProjectileSO projectileSO)
    {
        Vector2 direction = CalculateDir(turretWeapon);

        // 生成子弹
        Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
            projectileSO, 
            turretWeapon.Target, 
            turretWeapon.ProjectileCreatPos.position,
            direction,
            turretWeapon);
        Projectile.ProjectileCreateFactory(projectileCreationParams);
    }


}

public interface ITurretFireStg
{
    public void FireStg(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO);
}

/// <summary>
/// ITurretFireStg 的抽象基类
/// </summary>
public abstract class TurretFireStrategyBase : MonoBehaviour, ITurretFireStg
{
    public abstract void FireStg(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO);

    protected Vector2 CalculateDir(TurretWeaponV2 turretWeaponV2)
    {
        if (turretWeaponV2.Target == null)
        {
            Debug.LogError("TurretWeaponV2.Target is null");
            return Vector2.up;
        }
        return turretWeaponV2.Target.position - turretWeaponV2.ProjectileCreatPos.position;
    }
}
