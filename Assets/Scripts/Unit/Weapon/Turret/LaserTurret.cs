using System;
using System.Collections.Generic;
using CustomInspector;
using QFramework;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;


/// <summary>
/// 激光炮塔 因为是持续伤害 至少必须要是开关模式的射击 而不是像普通的一样单次连续射击
/// 炮塔部分逻辑:
///     炮塔的旋转索敌 还是照旧
///     但是开火的时候是
/// </summary>
public class LaserTurret : AbstractTurret
{
    [SerializeField] private GameObject turret;
    [SerializeField] [ForceFill] private Transform projectileCreatePos;
    [SerializeField] [ReadOnly] private Transform target;

    [HorizontalLine("激光属性")]
    //[SerializeField, ReadOnly] private float timer = 0;
    //[SerializeField, ForceFill] private LineRenderer laser;
    //[SerializeField, ReadOnly] private ProjectileSO curProjectileSO;
    private ILaserFireStg laserFireStg;

    protected override void Awake()
    {
        base.Awake();
        laserFireStg = GetComponent<ILaserFireStg>();
    }

    private void Update()
    {
        FireStrategyPreFrame(Time.deltaTime);
    }

    public virtual void FireStrategyPreFrame(float deltaTime)
    {
        // 如果没有turretSO 就不进行射击
        if (turretSO == null) return;

        // 如果有目标判断目标有没有超过射击范围
        if (target != null &&
            (target.transform.position - (turret != null ? turret.transform.position : transform.position)).magnitude >
            turretSO.radius)
        {
            target = null; // 超过射击范围就不再索敌

            // 关闭激光 并重置timer
            //Debug.Log("超过射击范围");
            //timer = 0f;
            //laser.enabled = false;
            laserFireStg.CloseLaser(this);
        }

        // 如果炮塔当前没有目标 尝试获取目标
        if (target == null)
        {
            RaycastHit2D enemy = Physics2D.CircleCast(
                turret != null ? turret.transform.position : transform.position,
                turretSO.radius, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
            target = enemy.transform;
        }
        
        // 如果没有目标 关闭激光 并重置timer
        if (target == null)
        {
            //Debug.Log("没有目标");
            // 关闭激光 并重置timer
            //timer = 0f;
            //laser.enabled = false;
            laserFireStg.CloseLaser(this);
            return;
        }

        // 如果有目标尝试对目标的transform攻击
        // 瞄准
        this.SendCommand(new TurretRotateTurretCommand((target.position - transform.position).normalized, deltaTime,
            turret.transform, turretSO));

        // 如果激光已经是开着了 那就不管是否已经瞄准好了 开着就可以了
        // 不然就要检查是否瞄准了
        if (!laserFireStg.IsLaserOpen && this.SendCommand(new TurretCheckAimCommand()
            {
                turret = turret.transform,
                target = target,
                allowAngle = 5f
            }))
        {
            // 瞄准好了 打开激光
            laserFireStg.OpenLaser(this);
        }

        // 执行update
        laserFireStg.LaserUpdate(this, deltaTime);

        // 逻辑为 获取到弹药(弹药直接 - 1) 开始计算 timer 并且 target 必须要是同一个 比如在同一个物体上
        // 获取弹药并且开始计时 计时满一个fire gap就扣血计算伤害
        // 如果是timer 是在0 那就获取弹药 并且生成 laserProjectile (设置line render 的两端) 如果, timer 达到了fire gap 就开始计算伤害

        // 如果timer == 0 就获取弹药
        //if (timer == 0f)
        //{
        //    curProjectileSO = GetProjectile();
        //    Debug.Log($"激光获取子弹 : {curProjectileSO}");
        //    if (curProjectileSO != null)
        //    {
        //        // 先测试 不用projectile 用line render先替代
        //        //// 生成子弹
        //        //var laserProjectile = Projectile.ProjectileCreateFactory(new Projectile.ProjectileCreationParams(
        //        //    projectileSO, GetTarget(), GetProjectileCreatePos(), GetFireDir(), this));

        //        // 设置line render 的两端
        //        laser.enabled = true;
        //        laser.positionCount = 2;
        //        laser.SetPosition(0, projectileCreatePos.position);
        //        laser.SetPosition(1, target.position);

        //        timer += deltaTime;
        //    }
        //}
        //else if (timer < turretSO.fireGap)
        //{
        //    timer += deltaTime;
        //    laser.positionCount = 2;
        //    laser.SetPosition(0, projectileCreatePos.position);
        //    laser.SetPosition(1, target.position);
        //}
        //else
        //{
        //    laser.positionCount = 2;
        //    laser.SetPosition(0, projectileCreatePos.position);
        //    laser.SetPosition(1, target.position);
        //    timer = 0f;

        //    // 计算伤害
        //    if (target.TryGetComponent<IBeDamage>(out IBeDamage beDamage))
        //    {
        //        beDamage.BeDamage(new DamageInfo(curProjectileSO.damage, this));
        //    }
        //}

    }

    public override Transform GetTarget()
    {
        return target;
    }

    public override Vector2 GetFireDir()
    {
        return turret.transform.up;
    }

    public override Vector3 GetProjectileCreatePos()
    {
        return projectileCreatePos.position;
    }

    public override void SetState(bool value)
    {
        base.SetState(value);
        laserFireStg.CloseLaser(this);
    }
}