using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplorTriggerStg : ProjectileTriggerStgBase
{
    

    public override void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << projectileTriggerParameters.other.gameObject.layer) & projectileTriggerParameters.projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // 触发爆炸
            // 检索爆炸范围所有的敌人
            RaycastHit2D[] result = Physics2D.CircleCastAll(
                projectileTriggerParameters.projectile.transform.position, 
                projectileTriggerParameters.projectile.ProjectileSO.explosionRadius, 
                Vector2.zero, 
                0f, 
                projectileTriggerParameters.projectile.ProjectileSO.targetLayer);

            foreach (RaycastHit2D item in result)
            {
                var beDamage = item.collider.gameObject.GetComponent<IBeDamage>();
                if (beDamage != null)
                {
                    DamageInfo damageInfo = new DamageInfo(
                        projectileTriggerParameters.projectile.ProjectileSO.damage, 
                        projectileTriggerParameters.projectile.Creater);

                    beDamage.BeDamage(damageInfo);
                }
            }

            // 触发特效
            ShowFX(projectileTriggerParameters.projectile.ProjectileSO.explosionRadius);

            projectileTriggerParameters.afterTrigger?.Invoke();

            // 销毁子弹
            projectileTriggerParameters.projectile.Destroy();
        }
    }
}
