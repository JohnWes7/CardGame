using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class TurretFixed : AbstractTurret
{
    [SerializeField, ForceFill]
    private Transform projectileCreatePos;
    [SerializeField, ReadOnly]
    private float timer;

    [SerializeField] private float angle;
    [SerializeField, ReadOnly] private bool isFire;

    [HorizontalLine("Stg")]
    [SerializeField, ReadOnly] private MonoInterface<ITurretFireStg> turretFireStg;

    public override Transform GetTarget()
    {
        return null;
    }

    public override Vector2 GetFireDir()
    {
        return transform.up;
    }

    public override Vector3 GetProjectileCreatePos()
    {
        return projectileCreatePos.position;
    }

    protected override void Awake()
    {
        base.Awake();
        timer = 0;
        turretFireStg.InterfaceObj = GetComponent<ITurretFireStg>();
    }

    private void Update()
    {
        FireStrategyPreFrame(deltaTime:Time.deltaTime);
    }

    public virtual void FireStrategyPreFrame(float deltaTime)
    {
        // 如果没有turretSO 就不进行射击
        if (turretSO == null)
        {
            return;
        }

        timer += deltaTime;
        if (timer < turretSO.fireGap)
        {
            return;
        }

        // 根据前方扇形判断是否有敌人 如果有敌人就开火
        // 获取前方所有的目标物体
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, turretSO.radius, LayerMask.GetMask("Enemy"));

        foreach (var target in targetsInViewRadius)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;

            // 判断目标物体是否在扇形范围内
            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                // Debug.Log("目标物体在扇形范围内：" + target.name);
                isFire = true;
                break;
            }
            isFire = false;
        }
    

        if (isFire)
        {
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
