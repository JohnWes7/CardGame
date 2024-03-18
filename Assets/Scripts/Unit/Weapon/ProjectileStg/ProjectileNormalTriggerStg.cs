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
                // 创造damageInfo并执行扣血
                DamageInfo damageInfo = new DamageInfo(
                    projectileTriggerParameters.projectile.ProjectileSO.damage,
                    projectileTriggerParameters.projectile.Creater
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
            }
        }
    }
}

public abstract class ProjectileTriggerStgBase : MonoBehaviour, IProjectileTriggerStrategy, IPoolComponent
{
    [HorizontalLine("特效")]
    [SerializeField, AssetsOnly]
    protected GameObject fxPrefab;

    [SerializeField]
    protected bool showFX = true;
    [SerializeField]
    protected bool scaleFXByExplosionRadius = true;
    [SerializeField]
    protected Vector3 scaleMulty = Vector3.one;

    [HorizontalLine("触发数据")]
    // 子弹穿透
    [SerializeField]
    protected List<GameObject> triggerTarget = new();

    public abstract void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters);

    public virtual void ShowFX(float explosionRadius)
    {
        if (showFX && fxPrefab)
        {
            // 从对象池中拿取实体
            var fxInstance = Johnwest.ObjectPoolManager.Instance.GetPool(fxPrefab).Get();
            fxInstance.transform.position = transform.position;

            // 调整大小
            Vector3 vector3 = scaleFXByExplosionRadius ? scaleMulty * explosionRadius * 2 : scaleMulty;
            fxInstance.transform.localScale = vector3;

            // 添加自动回收组件
            var autoReturnPool = fxInstance.AddComponent<AutoReturnPool>();
            autoReturnPool.Initialize(fxPrefab, 5f);
        }
    }

    public virtual void Initialize(object args) 
    {
        triggerTarget.Clear();
    }
}
