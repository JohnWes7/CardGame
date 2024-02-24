using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticProjectile : Projectile
{
    private IProjectileTriggerStrategy triggerStrategy;
    private IProjectileBehaviorStg behaviorStg;

    private void Start()
    {
        behaviorStg = new NormalBehavior(this);
        triggerStrategy = new CollisionTriggeredCommonFuzesStg();

        behaviorStg.Initialize();
    }

    private void Update()
    {
        behaviorStg.UpdatePreDeltaTime(Time.deltaTime);
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

