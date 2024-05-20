using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using Unity.VisualScripting;

/// <summary>
/// 负责炮塔激发前的行为逻辑
/// </summary>
public class TurretWeaponV2 : AbstractTurret
{
    [SerializeField] private GameObject turret;
    [SerializeField, ForceFill] private Transform projectileCreatePos;
    [SerializeField, ReadOnly] private Transform target;
    [SerializeField, ReadOnly] private float timer;

    // 逻辑组件
    [HorizontalLine("Stg")]
    [SerializeField, ReadOnly] private MonoInterface<ITurretFireStg> turretFireStg;

    public Transform Target { get => target; set => target = value; }
    public Transform ProjectileCreatePos { get => projectileCreatePos; set => projectileCreatePos = value; }

    protected override void Awake()
    {
        base.Awake();
        timer = 0;
        turretFireStg.InterfaceObj = GetComponent<ITurretFireStg>();
    }

    private void Update()
    {
        FireStrategyPreFrame(Time.deltaTime);
    }

    public virtual void FireStrategyPreFrame(float deltaTime)
    {
        // 如果没有turretSO 就不进行射击
        if (turretSO == null)
        {
            timer = 0;
            return;
        }

        // 如果有目标判断目标有没有超过射击范围
        if (target != null && (target.transform.position - (turret != null ? turret.transform.position : transform.position)).magnitude > turretSO.radius)
        {
            target = null; // 超过射击范围就不再索敌
        }

        // 如果炮塔当前没有目标 尝试获取目标
        if (target == null)
        {
            RaycastHit2D enemy = Physics2D.CircleCast(
                turret != null ? turret.transform.position : transform.position,
                turretSO.radius, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
            target = enemy.transform;
        }

        if (timer < turretSO.fireGap)
        {
            timer += deltaTime;
            if (target != null)
            {
                RotateTurret(target.position - (turret != null ? turret.transform.position : transform.position), deltaTime);
            }
            return;
        }


        // 如果有目标尝试对目标的transform攻击
        if (target != null)
        {
            // 瞄准
            RotateTurret(target.position - (turret != null ? turret.transform.position : transform.position), deltaTime);
            // 检查是否瞄准了
            if (CheckTakeAim())
            {
                //Debug.Log("已近瞄准");
                // 如果已经瞄准了就尝试获取弹药
                ProjectileSO projectileInfo = GetProjectile();
                if (projectileInfo != null)
                {
                    // 改用策略模式
                    if (turretFireStg.InterfaceObj != null)
                    {
                        turretFireStg.InterfaceObj.FireStg(this, projectileInfo);
                    }

                    timer = 0;
                }
            }
        }
    }

    protected virtual void RotateTurret(Vector3 dest, float deltaTime)
    {
        float angle = Mathf.Atan2(dest.y, dest.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, TurretSO.rotateSpeed * deltaTime);
    }

    protected virtual bool CheckTakeAim()
    {
        if (target != null)
        {
            Vector3 dest = target.position - (turret != null ? turret.transform.position : transform.position);
            Vector3 curAim = turret.transform.up;
            dest.z = 0;
            curAim.z = 0;
            if (Mathf.Abs(Vector3.Angle(dest, curAim)) < 5)
            {
                return true;
            }
        }

        return false;
    }

    public override Transform GetTarget()
    {
        return target;
    }

    public override Vector2 GetFireDir()
    {
        var dir = target.transform.position - transform.position;
        dir.z = 0;
        return dir.normalized;
    }

    public override Vector3 GetProjectileCreatePos()
    {
        return projectileCreatePos.position;
    }
}
