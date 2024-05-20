using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileTriggerParameters
{
    public Projectile projectile;
    public Collider2D other;
    public Action afterTrigger;
}

public interface IProjectileTriggerStrategy
{
    public void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters);

}

public interface IProjectileBehaviorStg
{
    void UpdatePreDeltaTime(float deltaTime);
}

public interface IPoolComponent
{
    void Initialize(object args);
}

/// <summary>
/// 普通子弹移动策略
/// </summary>
public class NormalBehavior : IProjectileBehaviorStg
{
    private Projectile context;
    private Vector2 velocity;
    private float durationTimer;


    public void Initialize(Projectile projectile)
    {
        context = projectile;

        // 初始化值
        durationTimer = 0;
        velocity = context.Direction;

        // 调整朝向
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        context.transform.rotation = rotation;

        // 归一化速度向量
        velocity = velocity.normalized;
    }

    /// <summary>
    /// 每一个时间节点调用一次 传入时间节点之间的间隔
    /// </summary>
    /// <param name="deltaTime"></param>
    public void UpdatePreDeltaTime(float deltaTime)
    {
        durationTimer += deltaTime;
        if (durationTimer > Projectile.PROJECTILE_DURATION)
        {
            context.Destroy();
        }
        context.transform.position += new Vector3(velocity.x, velocity.y) * context.ProjectileSO.speed * deltaTime;
    }
}

/// <summary>
/// 碰撞触发普通引信
/// </summary>
public class CollisionTriggeredCommonFuzesStg : IProjectileTriggerStrategy
{
    public void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << projectileTriggerParameters.other.gameObject.layer) & projectileTriggerParameters.projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // LogUtilsXY.LogOnPos($"Hit tag:{other.tag}", projectile.transform.position);
            // 执行攻击
            var beDamage = projectileTriggerParameters.other.GetComponent<IBeDamage>();
            if (beDamage != null)
            {
                // 创造damageInfo
                DamageInfo damageInfo = new DamageInfo(projectileTriggerParameters.projectile.ProjectileSO.damage, projectileTriggerParameters.projectile.Creator);

                beDamage.BeDamage(damageInfo);
                projectileTriggerParameters.afterTrigger?.Invoke();
                projectileTriggerParameters.projectile.Destroy();
            }
        }
    }
}

/// <summary>
/// 碰撞后触发爆炸的引信
/// </summary>
public class CollisionTriggeredExplosionFuzesStg : IProjectileTriggerStrategy
{
    public void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << projectileTriggerParameters.other.gameObject.layer) & projectileTriggerParameters.projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // 触发爆炸
            // 检索爆炸范围所有的敌人
            RaycastHit2D[] result = Physics2D.CircleCastAll(projectileTriggerParameters.projectile.transform.position, projectileTriggerParameters.projectile.ProjectileSO.explosionRadius, Vector2.zero, 0f, projectileTriggerParameters.projectile.ProjectileSO.targetLayer);
            foreach (RaycastHit2D item in result)
            {
                var beDamage = item.collider.gameObject.GetComponent<IBeDamage>();
                if (beDamage != null)
                {
                    DamageInfo damageInfo = new DamageInfo(projectileTriggerParameters.projectile.ProjectileSO.damage, projectileTriggerParameters.projectile.Creator);

                    beDamage.BeDamage(damageInfo);
                }
            }

            // 触发特效
            projectileTriggerParameters.afterTrigger?.Invoke();

            // 销毁子弹
            projectileTriggerParameters.projectile.Destroy();
        }
    }
}