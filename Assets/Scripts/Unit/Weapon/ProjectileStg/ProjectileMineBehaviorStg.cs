using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMineBehaviorStg : AbstractProjectileBehaviorStgBase
{
    public float moveDamp = 0.5f;
    public Projectile context;
    public Vector2 velocity;

    public override void Initialize(object args)
    {
        if (args is not Projectile projectile1) return;
        context = projectile1;

        // 初始化值
        velocity = context.Direction;

        // 调整朝向
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        context.transform.rotation = rotation;

        // 归一化速度向量
        velocity = velocity.normalized * context.ProjectileSO.speed;
    }

    public override void UpdatePreDeltaTime(float deltaTime)
    {
        transform.position = transform.position + new Vector3(velocity.x, velocity.y) * deltaTime;
        velocity = Vector2.Lerp(velocity, Vector2.zero, moveDamp * deltaTime);
    }
}
