using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

/// <summary>
/// 核心单位
/// </summary>
public class CoreUnit : UnitObject
{
    public override void BeDamage(DamageInfo projectile)
    {
        base.BeDamage(projectile);

        // 如果血量低于0触发游戏结束事件
        if (curHP <= 0)
        {
            this.SendCommand(new CoreUnitTriggerLostEventCommand(this));
        }
    }
}
