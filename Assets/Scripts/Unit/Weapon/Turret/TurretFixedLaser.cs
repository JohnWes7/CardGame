using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;

public class TurretFixedLaser : AbstractTurret
{
    [SerializeField, ForceFill]
    private Transform projectileCreatePos;
    [SerializeField]
    private float angle;
    [SerializeField, ReadOnly]
    private bool isFire;
    [SerializeField, ReadOnly]
    private bool startFire;
    [SerializeField, ReadOnly]
    private float laserDamageTimer;

    [HorizontalLine("固定激光整合的子弹")]
    [SerializeField, ReadOnly]
    private LaserProjectileBase laserProjectileInstance;

    public override void SetState(bool value)
    {
        base.SetState(value);
        if (!value && laserProjectileInstance != null)
        {
            laserProjectileInstance.CloseLaser(this);
        }
    }

    public override Vector2 GetFireDir()
    {
        return transform.up;
    }

    public override Vector3 GetProjectileCreatePos()
    {
        return projectileCreatePos.position;
    }

    public override Transform GetTarget()
    {
        return null;
    }

    public void Update()
    {
        FireStrategyPreFrame(Time.deltaTime);
    }

    public void FireStrategyPreFrame(float deltaTime)
    {
        // 检查
        if (turretSO == null)
        {
            return;
        }

        // 根据前方扇形判断是否有敌人 如果有敌人就开火
        // 获取前方所有的目标物体
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(
            transform.position,
            turretSO.radius,
            LayerMask.GetMask("Enemy"));

        // 遍历检测一遍
        bool flag = false;
        foreach (var target in targetsInViewRadius)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;

            // 判断目标物体是否在扇形范围内
            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                flag = true;
                break;
            }
        }

        // 如果检测到目标物体
        if (flag)
        {    
            // 如果是重新打开激光那么则设置startFire为true 之后要调用 激光的open
            if (isFire == false)
            {
                startFire = true;
            }
            isFire = true;
        }
        else
        {
            isFire = false;
        }

        // 开火
        if (isFire)
        {
            // 如果startfire 为true则打开激光 并且重置一些属性
            if (startFire)
            {
                // Debug.Log("打开激光");
                laserDamageTimer = 0;                
                if (laserProjectileInstance != null) laserProjectileInstance.OpenLaser(this);
                startFire = false;
            }

            // 每一帧都检查子弹是否正确 如果需要改变就替换laserProjectileInstance
            CheckProjectile();

            // 激光更新
            laserDamageTimer += deltaTime;
            if (laserProjectileInstance != null) laserProjectileInstance.LaserUpdate(this, deltaTime);

            // 如果timer 大于firegp 结算伤害
            if (laserDamageTimer >= turretSO.fireGap)
            {
                // 结算伤害
                if (laserProjectileInstance != null) laserProjectileInstance.DoDamage(this);
                // 减少弹药
                GetProjectile();
                laserDamageTimer = 0;
            }
        }
        else
        {
            if (laserProjectileInstance != null) laserProjectileInstance.CloseLaser(this);
        }
    }

    private void CheckProjectile()
    {
        // 获得当前的子弹类型
        var projectileSO = PeekProjectile();
        if (projectileSO == null)
        {
            Debug.LogError("没有子弹类型");
            return;
        }

        // 如果 projectile 是null 或者不是当前需要的子弹 就生成一个
        if (laserProjectileInstance == null || laserProjectileInstance.ProjectileSO != projectileSO)
        {
            // Debug.Log($"更换激光子弹 {laserProjectileInstance?.ProjectileSO} -> {projectileSO}");
            // 如果之前有激光子弹 那么销毁之前的
            if (laserProjectileInstance != null)
            {
                laserProjectileInstance.Destroy();
            }

            var laser = Projectile.ProjectileCreateFactory(new Projectile.ProjectileCreationParams()
            {
                projectileSO = projectileSO,
                direction = GetFireDir(),
                target = GetTarget(),
                creator = this
            });

            if (laser is LaserProjectileBase laserProjectileBase)
            {
                laserProjectileInstance = laserProjectileBase;
            }
            else
            {
                Debug.LogError("生成的激光子弹不是LaserProjectileBase类型");
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, turretSO.radius);

        Vector3 forward = transform.up * turretSO.radius;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, angle / 2) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -angle / 2) * forward;

        Gizmos.DrawLine(transform.position, transform.position + forward);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
    }

}