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
    public override void FireStg(AbstractTurret turretWeapon, ProjectileSO projectileSO)
    {
        Vector2 direction = turretWeapon.GetFireDir();

        // 生成一颗子弹
        Projectile.ProjectileCreationParams projectileCreationParams = new Projectile.ProjectileCreationParams(
            projectileSO, 
            turretWeapon.GetTarget(), 
            turretWeapon.GetProjectileCreatePos(),
            direction,
            turretWeapon);

        // 触发事件 并等待事件修改参数
        FireEventArgs para = new FireEventArgs {
            CreatePosition = turretWeapon.GetProjectileCreatePos(),
            Target = turretWeapon.GetTarget(),
            Direction = direction,
            ProjectileSO = projectileSO, 
            creator = turretWeapon,
            ProjectileCreationParams = new List<Projectile.ProjectileCreationParams> { projectileCreationParams }, 
        };

        // 触发事件
        RaiseOnFire(para);
        
        if (para.ProjectileCreationParams.Count == 0)
        {
            Debug.LogError("ProjectileCreationParams is empty");
            return;
        }

        //从触发事件后的para中获取参数 并生成子弹
        StartCoroutine(Fire(para));
    }

    public IEnumerator Fire(FireEventArgs args)
    {
        // 一共执行args.burstCount次
        for (int i = 0; i < args.burstCount; i++)
        {
            //从触发事件后的para中获取参数 并生成子弹
            foreach (Projectile.ProjectileCreationParams item in args.ProjectileCreationParams)
            {
                Projectile.ProjectileCreateFactory(item);
            }

            yield return new WaitForSeconds(args.burstDelay);
        }
    }
}



public interface ITurretFireStg
{
    public void FireStg(AbstractTurret turretWeaponV2, ProjectileSO projectileSO);
}

public interface IOnFire 
{
    public PriorityEventManager<FireEventArgs> priorityEventManager { get; }
}



// 中介者模式 用来扩展装备更改修改 ProjectileCreationParams 的参数
public class FireEventArgs : EventArgs
{
    public Vector3 CreatePosition = Vector3.zero;
    /// <summary>
    /// 注意target有可能是null
    /// </summary>
    public Transform Target;
    public Vector2 Direction;
    public ProjectileSO ProjectileSO;
    public List<Projectile.ProjectileCreationParams> ProjectileCreationParams = new List<Projectile.ProjectileCreationParams>();
    public object creator;

    // 连射逻辑属性
    public int burstCount = 1;
    public float burstDelay = 0.1f;
}



/// <summary>
/// 继承 ITurretFireStg 的抽象基类
/// 用于实现不同的开火策略
/// </summary>
public abstract class TurretFireStrategyBase : MonoBehaviour, ITurretFireStg, IOnFire
{
    public PriorityEventManager<FireEventArgs> priorityEventManager { get; } = new PriorityEventManager<FireEventArgs>();

    public abstract void FireStg(AbstractTurret turretWeaponV2, ProjectileSO projectileSO);

    /// <summary>
    /// 供子类调用的触发事件的方法
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected void RaiseOnFire(FireEventArgs e)
    {
        priorityEventManager.Invoke(this, e);
    }
}
