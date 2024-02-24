using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTriggerStrategy
{
    public void OnTriggerEnter(Projectile projectile, Collider2D other);

}

public interface IProjectileBehaviorStg
{
    void Initialize();
    void UpdatePreDeltaTime(float deltaTime);
}

/// <summary>
/// 普通子弹移动
/// </summary>
public class NormalBehavior : IProjectileBehaviorStg
{
    private Projectile context;
    private Vector2 velocity;
    private float durationTimer = 0f;

    public NormalBehavior(Projectile projectile)
    {
        this.context = projectile;
    }

    public void Initialize()
    {
        Debug.Log($"create projectile: {context.ProjectileSO} target: {context.Target}");
        // 计算打击的方向
        velocity = context.Target.position - context.transform.position;

        // 调整朝向
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        context.transform.rotation = rotation;

        // 归一化速度向量
        velocity = velocity.normalized;
    }

    public void UpdatePreDeltaTime(float deltaTime)
    {
        durationTimer += deltaTime;
        if (durationTimer > Projectile.PROJECTILE_DURATION)
        {
            Object.Destroy(context);
        }
        context.transform.position += new Vector3(velocity.x, velocity.y) * context.ProjectileSO.speed * deltaTime;
    }
}

/// <summary>
/// 碰撞触发普通引信
/// </summary>
public class CollisionTriggeredCommonFuzesStg : IProjectileTriggerStrategy
{
    public void OnTriggerEnter(Projectile projectile, Collider2D other)
    {
        if (projectile.ProjectileSO.targetTag.Contains(other.tag))
        {
            LogUtilsXY.LogOnPos($"Hit tag:{other.tag}", projectile.transform.position);
            // 执行攻击
            var bedamgage = other.GetComponent<IBeDamage>();
            if (bedamgage != null)
            {
                bedamgage.BeDamage(projectile);
                Object.Destroy(projectile.gameObject);
            }
        }
    }
}
