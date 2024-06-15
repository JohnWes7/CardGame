using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

/// <summary>
/// 导弹子弹行为
/// 导弹的索敌和追踪目标
/// 如果目标消失 则开始重新索敌, 如果重新索敌不到则朝最后的方向保持加速飞行
/// </summary>
public class ProjectileMissileBehaviorStg : AbstractProjectileBehaviorStgBase
{
    [SerializeField] [ReadOnly] private Projectile context;
    [SerializeField] [ReadOnly] private float currentSpeed = 0f; // 当前速度
    [SerializeField] private float startSpeed = 0f; // 初始速度
    [SerializeField] private float acceleration = 10f; // 加速度
    [SerializeField] private float redirectionRadius = 10f; // 重定向半径


    public override void Initialize(object args)
    {
        if (args is Projectile projectile) context = projectile;

        currentSpeed = startSpeed; // 初始化速度为 startSpeed 
    }

    public override void UpdatePreDeltaTime(float deltaTime)
    {
        // 模拟导弹的运动轨迹
        if (context == null) return;

        if (context.Target == null)
        {
            // 执行重定向搜索目标
            Collider2D colliders = Physics2D.OverlapCircle(context.transform.position, redirectionRadius,
                context.ProjectileSO.targetLayer);
            if (colliders != null)
            {
                context.Target = colliders.transform;
            }
            else
            {
                context.Target = null;
            }
        }

        // 不管那种移动都需要更新速度
        // 根据加速度和deltaTime更新当前速度，但不超过ProjectileSO中定义的最大速度
        currentSpeed += acceleration * deltaTime;
        currentSpeed = Mathf.Min(currentSpeed, context.ProjectileSO.speed);

        // 没有目标时候的移动
        if (context.Target == null)
        {
            // 如果没有目标则朝当前方向加速飞行
            context.transform.position += new Vector3(context.transform.up.x, context.transform.up.y) *
                                          context.ProjectileSO.speed * deltaTime;
            return;
        }

        #region 有目标的移动
        // 有目标的移动
        Vector2 directionToTarget = (context.Target.position - context.transform.position).normalized;

        // 使用插值方法平滑转向目标
        // 首先，计算目标方向的角度
        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;

        // 当前导弹的朝向
        float currentAngle = context.transform.eulerAngles.z;

        // 使用Lerp插值计算新的角度，模拟惯性效果
        // 越近 插值速率越大
        float t = redirectionRadius / Vector2.Distance(context.transform.position, context.Target.position) + 1;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, deltaTime * 5 * t); // 5是插值速率，可以调整以适应游戏的需要

        // 更新导弹朝向
        Quaternion rotation = Quaternion.Euler(0f, 0f, newAngle);
        context.transform.rotation = rotation;

        // 更新导弹位置
        Vector2 currentDirection = context.transform.up;
        context.transform.position += new Vector3(currentDirection.x, currentDirection.y, 0) * currentSpeed * deltaTime;
        #endregion
    }
}