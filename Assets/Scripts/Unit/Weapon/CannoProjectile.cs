using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class CannoProjectile : Projectile
{
    private IProjectileTriggerStrategy triggerStrategy;
    private IProjectileBehaviorStg behaviorStg;
    [SerializeField, AssetsOnly, ForceFill]
    private GameObject fxPrefab;

    public override void Initialize()
    {
        base.Initialize();

        behaviorStg ??= new NormalBehavior(this);
        triggerStrategy ??= new CollisionTriggeredExplosionFuzesStg();

        behaviorStg.Initialize();
    }

    private void Update()
    {
        behaviorStg.UpdatePreDeltaTime(Time.deltaTime);
    }


    /// <summary>
    /// 现在这里用SO里面的layer来判断是否击中 如果之后需要更多的方法因为SO 不好绑定策略模式
    /// 可行的方法: 
    /// 1 再弄个策略模式 在start里面用来复用 先尝试策略模式
    /// 2 用继承然后重写
    /// 
    /// 3 建一个生产stratage的factory 根据SO里面的值选择使用什么strategy
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileTriggerParameters projectileTriggerParameters = new ProjectileTriggerParameters
        {
            projectile = this,
            other = collision,
            triggerFX = () =>
            {
                if (fxPrefab)
                {
                    //Debug.Log("生成fx");
                    // 从对象池中拿取实体
                    var fxInstance = Johnwest.ObjectPoolManager.Instance.GetPool(fxPrefab).Get();
                    fxInstance.transform.position = transform.position;

                    fxInstance.transform.localScale = new Vector3(ProjectileSO.explosionRadius * 2, ProjectileSO.explosionRadius * 2, ProjectileSO.explosionRadius * 2);

                    // 添加自动回收组件
                    var autoReturnPool = fxInstance.AddComponent<AutoReturnPool>();
                    autoReturnPool.Initialize(fxPrefab, 5f);
                }
            }
        };

        triggerStrategy.TriggerInvoke(projectileTriggerParameters);
    }

}

