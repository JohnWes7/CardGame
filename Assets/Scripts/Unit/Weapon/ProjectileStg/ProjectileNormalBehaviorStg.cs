using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class ProjectileNormalBehaviorStg : AbstractProjectileBehaviorStgBase
{
    [SerializeField, ReadOnly]
    private Projectile context;
    [SerializeField, ReadOnly]
    private Vector2 velocity;
    [SerializeField, ReadOnly]
    private float durationTimer;

    public override void Initialize(object projectile)
    {
        if (projectile is not Projectile projectile1) return;
        context = projectile1;

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

    public override void UpdatePreDeltaTime(float deltaTime)
    {
        durationTimer += deltaTime;
        if (durationTimer > Projectile.PROJECTILE_DURATION)
        {
            context.Destroy();
        }
        context.transform.position += new Vector3(velocity.x, velocity.y) * context.ProjectileSO.speed * deltaTime;
    }
}

public abstract class AbstractProjectileBehaviorStgBase : MonoBehaviour, IProjectileBehaviorStg, IPoolComponent
{
    public abstract void Initialize(object args);
    public abstract void UpdatePreDeltaTime(float deltaTime);
}

