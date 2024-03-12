using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticProjectile : Projectile
{
    private IProjectileTriggerStrategy triggerStrategy;
    private IProjectileBehaviorStg behaviorStg;


    public override void Initialize()
    {
        base.Initialize();

        behaviorStg ??= new NormalBehavior(this);
        triggerStrategy = GetComponent<IProjectileTriggerStrategy>();

        behaviorStg.Initialize();
    }

    private void Update()
    {
        behaviorStg.UpdatePreDeltaTime(Time.deltaTime);
    }



    /// <summary>
    /// 组件策略模式
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 判断是否有策略模式
        if (triggerStrategy == null)
        {
            Debug.LogError("ProjectileTriggerStrategy is null");
            return;
        }

        ProjectileTriggerParameters projectileTriggerParameters = new ProjectileTriggerParameters{
            projectile = this,
            other = collision
        };

        triggerStrategy.TriggerInvoke(projectileTriggerParameters);
    }
}

