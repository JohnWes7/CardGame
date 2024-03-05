using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public abstract class EnemyTurret : MonoBehaviour
{
    [SerializeField, ReadOnly]
    protected Transform target;
    [SerializeField, ForceFill, AssetsOnly]
    protected EnemyTurretSO enemyTurretSO;
    [SerializeField, ReadOnly]
    protected float fireGapTimer;
    [SerializeField, ForceFill]
    protected Transform firePos;

    protected virtual void Update()
    {
        ShootPreTimeTick(Time.deltaTime);
    }

    public virtual bool TargetOutRange()
    {
        if (target)
        {
            Vector3 distance = target.transform.position - transform.position;
            if (distance.magnitude > enemyTurretSO.radius)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        return true;
    }

    public virtual Transform SearchTarget()
    {
        // 随便尝试对范围内一个飞船组件或者是飞船核心为目标开始开火
        var castResult = Physics2D.CircleCast(transform.position, enemyTurretSO.radius, Vector2.zero, 0f, enemyTurretSO.searchTargetLayer);
        if (castResult.collider != null)
        {
            return castResult.collider.transform;
        }

        return null;
    }

    /// <summary>
    /// 如果已经瞄准好了则返回true 表示可以射击 如果没有则返回false
    /// </summary>
    /// <returns></returns>
    public virtual bool AimTarget(float deltaTime)
    {
        if (target != null)
        {
            // 计算炮塔指向目标的方向
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.z = 0f; // 将Z轴置为0，使得炮塔只在水平方向旋转

            // 计算当前方向与目标方向之间的角度
            float angleToTarget = Vector3.Angle(transform.up, directionToTarget);

            // 计算目标方向的旋转
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f);

            // 使用 Quaternion.RotateTowards 来平滑地调整旋转
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, enemyTurretSO.rotateSpeed * deltaTime);

            // 应用新的旋转
            transform.rotation = newRotation;

            // 如果角度小于5度，表示已经对准目标
            if (angleToTarget < 3f)
            {
                // 应用新的旋转
                return true;
            }
        }
        return false; // 如果没有目标，则返回false
    }

    public virtual void ShootPreTimeTick(float deltaTime)
    {
        if (!enemyTurretSO)
        {
            return;
        }

        if (fireGapTimer < enemyTurretSO.fireGap)
        {
            fireGapTimer += deltaTime;
        }
        else
        {
            // 如果target的layer不在searchTargetLayer中则取消target
            if (target && (enemyTurretSO.searchTargetLayer & (1 << target.gameObject.layer)) == 0)
            {
                target = null;
            }

            // 如果当前有目标检查目标是否还在半径范围内 如果不在范围内则取消target
            if (target && TargetOutRange())
            {
                target = null;
            }

            // 如果没有目标就搜索目标
            if (target == null)
            {
                target = SearchTarget();
            }

            // 如果还是没有目标那么结束发射
            if (!target)
            {
                return;
            }

            // 进行发射准备
            if (AimTarget(deltaTime))
            {
                Projectile.ProjectileCreateFactory(enemyTurretSO.projectileSO, target, firePos.position, this);
                fireGapTimer = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyTurretSO)
        {
            Gizmos.color = Color.white; // 设置 Gizmos 颜色
            Gizmos.DrawWireSphere(transform.position, enemyTurretSO.radius); // 绘制表示射程范围的圆形
        }
    }
}
