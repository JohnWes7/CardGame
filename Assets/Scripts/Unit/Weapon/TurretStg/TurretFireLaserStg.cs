using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public interface ILaserFireStg
{
    bool IsLaserOpen { get; }
    void CloseLaser(AbstractTurret turretWeapon);
    void LaserUpdate(AbstractTurret turretWeapon, float deltaTime);
    void OpenLaser(AbstractTurret turretWeapon);
}

public class TurretFireLaserStg : MonoBehaviour, ILaserFireStg
{
    [SerializeField, ReadOnly] private bool isLaserOpen = false;
    [SerializeField, ReadOnly] private float timer = 0;
    [SerializeField, ReadOnly] private LaserProjectileBase laserProjectile;

    public bool IsLaserOpen => isLaserOpen;

    public void OpenLaser(AbstractTurret turretWeapon)
    {
        // 开启激光
        if (isLaserOpen)
        {
            return;
        }

        Debug.Log("打开激光");
        isLaserOpen = true;
        timer = 0;

        if (laserProjectile != null) laserProjectile.OpenLaser(turretWeapon);
    }

    public void CloseLaser(AbstractTurret turretWeapon)
    {
        // 关闭激光
        if (!isLaserOpen)
        {
            return;
        }

        Debug.Log("关闭激光");
        isLaserOpen = false;
        timer = 0;

        if (laserProjectile != null) laserProjectile.CloseLaser(turretWeapon);
    }

    public void LaserUpdate(AbstractTurret turretWeapon, float deltaTime)
    {
        if (!isLaserOpen)
            return;

        // 每打出一发激光子弹的时候 检查一次 (一个timer循环 或者说结算一次伤害的时候)
        if (timer == 0f)
        {
            // 检查当前发射的弹药
            var peekProjectileSO = turretWeapon.PeekProjectile();

            // 如果projectile 不是null 并且和需要发射的不是一种 那么destroy之前的并且替换为现在的
            if ((laserProjectile != null && laserProjectile.ProjectileSO != peekProjectileSO) || laserProjectile == null)
            {
                // 消除之前的
                if (laserProjectile != null) laserProjectile.Destroy();

                // 替换为现在的
                var temp = Projectile.ProjectileCreateFactory(new Projectile.ProjectileCreationParams()
                {
                    creator = turretWeapon,
                    direction = turretWeapon.GetFireDir(),
                    originPos = turretWeapon.GetProjectileCreatePos(),
                    projectileSO = peekProjectileSO,
                    target = turretWeapon.GetTarget()
                });
                try
                {
                    laserProjectile = temp as LaserProjectileBase;
                    timer += deltaTime;
                    laserProjectile.LaserUpdate(turretWeapon, deltaTime);
                }
                catch (Exception e)
                {
                    Debug.Log($" 无法转换为laser projectile 请检查 {turretWeapon.TurretSO} Exception: {e}");
                }
                return;
            }
        }

        // 检查laserProjectile是不是null 如果是就return 因为之后的操作 必须要laserProjectile
        if (laserProjectile == null)
        {
            Debug.LogWarning("laser Projectile is null");
            return;
        }

        // 执行每一帧都需要的操作
        laserProjectile.LaserUpdate(turretWeapon, deltaTime);
        timer += deltaTime;

        // 如果时间大于等于需要结算伤害的时间
        // 那么就结算伤害并且把时间调整为0
        if (timer >= turretWeapon.TurretSO.fireGap)
        {
            laserProjectile.DoDamage(turretWeapon);
            timer = 0;
            
            // 扣除弹药
            turretWeapon.GetProjectile();
        }

        
    }
}