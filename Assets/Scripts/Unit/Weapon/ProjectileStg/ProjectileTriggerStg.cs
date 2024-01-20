using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTriggerStrategy
{
    public void OnTriggerEnter(Projectile projectile, Collider2D other);

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
