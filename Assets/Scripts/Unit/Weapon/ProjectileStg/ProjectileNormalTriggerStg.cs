using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


public class ProjectileNormalTriggerStg : ProjectileTriggerStgBase
{
    public override void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << projectileTriggerParameters.other.gameObject.layer) & projectileTriggerParameters.projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // LogUtilsXY.LogOnPos($"Hit tag:{other.tag}", projectile.transform.position);
            // 执行攻击
            var bedamgage = projectileTriggerParameters.other.GetComponent<IBeDamage>();
            if (bedamgage != null)
            {
                // 创造damageInfo
                DamageInfo damageInfo = new DamageInfo(projectileTriggerParameters.projectile.ProjectileSO.damage, projectileTriggerParameters.projectile.Creater);

                bedamgage.BeDamage(damageInfo);
                projectileTriggerParameters.triggerFX?.Invoke();
                projectileTriggerParameters.projectile.Destroy();
            }
        }
    }
}

public abstract class ProjectileTriggerStgBase : MonoBehaviour, IProjectileTriggerStrategy
{
    [SerializeField, AssetsOnly]
    private GameObject fxPrefab;

    public abstract void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters);
}
