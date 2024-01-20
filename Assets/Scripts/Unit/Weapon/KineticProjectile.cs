using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticProjectile : Projectile
{
    [SerializeField] private Vector2 velocity;
    [SerializeField] private IProjectileTriggerStrategy triggerStrategy;
    [SerializeField] private float durationTimer;

    private void Start()
    {
        Debug.Log($"create projectile: {projectileSO} target: {target}");
        // 计算打击的方向
        velocity = target.position - transform.position;
        
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        velocity = velocity.normalized;
        triggerStrategy = new CollisionTriggeredCommonFuzesStg();
    }

    private void FixedUpdate()
    {
        durationTimer += Time.fixedDeltaTime;
        if (durationTimer > PROJECTILE_DURATION)
        {
            Destroy(gameObject);
        }
        transform.position += new Vector3(velocity.x, velocity.y) * projectileSO.speed;
    }

    /// <summary>
    /// 现在这里用SO里面的tag来判断是否击中 如果之后需要更多的方法因为SO 不好绑定策略模式
    /// 可行的方法: 
    /// 1 再弄个策略模式 在start里面用来复用 先尝试策略模式
    /// 2 用继承然后重写
    /// 
    /// 3 建一个生产stratage的factory 根据SO里面的值选择使用什么strategy
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerStrategy.OnTriggerEnter(this, collision);
    }
}

