using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileDoubleTroubleTriggerStg : ProjectileExplodeTriggerStg
{
    [SerializeField, ForceFill] private ProjectileSO childProjectileSO;
    [SerializeField, ReadOnly] private bool active = false; // 判断亡语是否触发过

    public override void Initialize(object args)
    {
        base.Initialize(args);

        // 初始化 从对象池中获取的
        active = false;
    }

    public override void TriggerInvoke(ProjectileTriggerParameters projectileTriggerParameters)
    {
        // 如果碰撞对象的 Layer 包含在目标 Layer 中
        if (((1 << projectileTriggerParameters.other.gameObject.layer) &
             projectileTriggerParameters.projectile.ProjectileSO.targetLayer.value) != 0)
        {
            // 设置判断
            if (active) return;
            active = true;

            // 分裂一个子弹
            Vector2 childDir = Random.insideUnitCircle.normalized; // 子弹的方向

            Projectile.ProjectileCreationParams param = new Projectile.ProjectileCreationParams
            {
                creator = projectileTriggerParameters.projectile.Creator, // 子弹创建者还是炮塔
                direction = childDir,   // 分裂方向
                projectileSO = childProjectileSO,   // 分裂的子弹SO
                target = projectileTriggerParameters.projectile.Target, // target
                originPos = projectileTriggerParameters.projectile.transform.position   // 创建位置就是现在子弹位置
            };
            Projectile.ProjectileCreateFactory(param); // 工厂创建
        }

        base.TriggerInvoke(projectileTriggerParameters);
    }
}
