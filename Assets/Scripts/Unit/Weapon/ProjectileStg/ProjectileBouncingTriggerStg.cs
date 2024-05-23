using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileBouncingTriggerStg : AbstractProjectileTriggerStgBase
{
    // 弹射搜索范围
    public float bounceSearchRadius = 10f;
    // 弹射数量 穿透数量也就是弹射数量

    public override void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << projectileTriggerParameters.other.gameObject.layer) & projectileTriggerParameters.projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // LogUtilsXY.LogOnPos($"Hit tag:{other.tag}", laserProjectile.transform.position);
            // 执行攻击
            var bedamgage = projectileTriggerParameters.other.GetComponent<IBeDamage>();
            if (bedamgage != null)
            {
                // 创造damageInfo并执行扣血
                DamageInfo damageInfo = new DamageInfo(
                    projectileTriggerParameters.projectile.ProjectileSO.damage,
                    projectileTriggerParameters.projectile.Creator
                );
                bedamgage.BeDamage(damageInfo);
                projectileTriggerParameters.afterTrigger?.Invoke();

                // 并且把受到攻击的对象加入到触发目标列表中
                triggerTarget.Add(projectileTriggerParameters.other.gameObject);

                // 如果触发目标数量大于等于穿透次数 则销毁子弹
                if (triggerTarget.Count >= projectileTriggerParameters.projectile.ProjectileSO.penetration)
                {
                    projectileTriggerParameters.projectile.Destroy();
                }

                // 弹跳部分 : 

                // 先搜索 bounceSearchRadius 范围内的enemy
                var results = Physics2D.CircleCastAll(
                    projectileTriggerParameters.projectile.transform.position,
                    bounceSearchRadius,
                    Vector2.zero,
                    0f,
                    projectileTriggerParameters.projectile.ProjectileSO.targetLayer);
                
                // 找一个不在 triggerTarget 中的enemy
                RaycastHit2D result = Array.Find(results, r => !triggerTarget.Contains(r.collider.gameObject));

                // 更改projectile的方向和target 并调用 behavior 的初始化让他重新根据projectile初始化并且向新的方向移动
                if (result.collider != null)
                {
                    projectileTriggerParameters.projectile.Target = result.collider.transform;
                    projectileTriggerParameters.projectile.Direction = (result.transform.position - projectileTriggerParameters.projectile.transform.position).normalized;
                    // 重新初始化
                    GetComponent<AbstractProjectileBehaviorStgBase>().Initialize(projectileTriggerParameters.projectile);
                }
                // 如果没有目标就随机弹射一个方向
                else
                {
                    projectileTriggerParameters.projectile.Target = null;
                    // 随机一个方向
                    Vector2 randomDir = Random.onUnitSphere;
                    projectileTriggerParameters.projectile.Direction = randomDir;
                    // 重新初始化
                    GetComponent<AbstractProjectileBehaviorStgBase>().Initialize(projectileTriggerParameters.projectile);
                }
            }
        }
    }
}
