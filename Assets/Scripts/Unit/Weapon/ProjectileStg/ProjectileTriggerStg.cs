using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTriggerStrategy
{
    public void OnTriggerEnter(Projectile projectile, Collider2D other, Action triggerFX = null);

}

public interface IProjectileBehaviorStg
{
    void Initialize();
    void UpdatePreDeltaTime(float deltaTime);
}

/// <summary>
/// 普通子弹移动策略
/// </summary>
public class NormalBehavior : IProjectileBehaviorStg
{
    private Projectile context;
    private Vector2 velocity;
    private float durationTimer;

    public NormalBehavior(Projectile projectile)
    {
        this.context = projectile;
    }

    public void Initialize()
    {
        Debug.Log($"create projectile: {context.ProjectileSO} target: {context.Target}");
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
    public void OnTriggerEnter(Projectile projectile, Collider2D other, Action triggerFX = null)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << other.gameObject.layer) & projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // LogUtilsXY.LogOnPos($"Hit tag:{other.tag}", projectile.transform.position);
            // 执行攻击
            var bedamgage = other.GetComponent<IBeDamage>();
            if (bedamgage != null)
            {
                // 创造damageInfo
                DamageInfo damageInfo = new DamageInfo(projectile.ProjectileSO.damage, projectile.Creater);

                bedamgage.BeDamage(damageInfo);
                triggerFX?.Invoke();
                projectile.Destroy();
            }
        }
    }
}

/// <summary>
/// 碰撞后触发爆炸的引信
/// </summary>
public class CollisionTriggeredExplosionFuzesStg : IProjectileTriggerStrategy
{
    public void OnTriggerEnter(Projectile projectile, Collider2D other, Action triggerFX = null)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << other.gameObject.layer) & projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // 触发爆炸
            // 检索爆炸范围所有的敌人
            RaycastHit2D[] result = Physics2D.CircleCastAll(projectile.transform.position, projectile.ProjectileSO.explosionRadius, Vector2.zero, 0f, projectile.ProjectileSO.targetLayer);
            foreach (RaycastHit2D item in result)
            {
                var beDamage = item.collider.gameObject.GetComponent<IBeDamage>();
                if (beDamage != null)
                {
                    DamageInfo damageInfo = new DamageInfo(projectile.ProjectileSO.damage, projectile.Creater);

                    beDamage.BeDamage(damageInfo);
                }
            }

            // 触发特效
            triggerFX?.Invoke();

            // 销毁子弹
            projectile.Destroy();
        }
    }
}