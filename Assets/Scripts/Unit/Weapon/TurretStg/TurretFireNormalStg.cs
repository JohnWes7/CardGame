using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFireNormalStg : TurretFireStrategyBase
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

        // 触发事件 并等待事件修改参数
        FireEventArgs para = new FireEventArgs {
            CreatePosition = turretWeapon.ProjectileCreatPos.position,
            Target = turretWeapon.Target,
            ProjectileSO = projectileSO, 
            ProjectileCreationParams = new List<Projectile.ProjectileCreationParams> { projectileCreationParams } };

        RaiseOnFire(para);

        //从触发事件后的para中获取参数 并生成子弹
        foreach (Projectile.ProjectileCreationParams item in para.ProjectileCreationParams)
        {
            Projectile.ProjectileCreateFactory(item);
        }
    }


}



public interface ITurretFireStg
{
    public void FireStg(TurretWeaponV2 turretWeaponV2, ProjectileSO projectileSO);
}

public interface IOnFire 
{
    //public event EventHandler<FireEventArgs> OnFire;
    public PriorityEventManager<FireEventArgs> priorityEventManager { get; }
}



// 中介者模式 用来扩展装备更改修改 ProjectileCreationParams 的参数
public class FireEventArgs : EventArgs
{
    public float delay = 0f;
    public Vector3 CreatePosition = Vector3.zero;
    public Transform Target;
    public ProjectileSO ProjectileSO;
    public List<Projectile.ProjectileCreationParams> ProjectileCreationParams = new List<Projectile.ProjectileCreationParams>();
}



/// <summary>
/// ITurretFireStg 的抽象基类
/// </summary>
public abstract class TurretFireStrategyBase : MonoBehaviour, ITurretFireStg, IOnFire
{
    //public event EventHandler<FireEventArgs> OnFire;
    public PriorityEventManager<FireEventArgs> priorityEventManager { get; } = new PriorityEventManager<FireEventArgs>();


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

    /// <summary>
    /// 供子类调用的触发事件的方法
    /// </summary>
    /// <param name="e"></param>
    protected void RaiseOnFire(FireEventArgs e)
    {
        priorityEventManager.Invoke(this, e);
    }
}
