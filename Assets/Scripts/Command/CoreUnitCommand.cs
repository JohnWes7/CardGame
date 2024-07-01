using UnityEngine;
using QFramework;

/// <summary>
/// 核心单位 触发游戏结束事件
/// </summary>
public class CoreUnitTriggerLostEventCommand : AbstractCommand
{
    public CoreUnit coreUnit;

    public CoreUnitTriggerLostEventCommand(CoreUnit coreUnit)
    {
        this.coreUnit = coreUnit;
    }

    protected override void OnExecute()
    {
        this.SendEvent<CoreUnitLostEvent>(new CoreUnitLostEvent(coreUnit));
    }
}

public struct CoreUnitLostEvent
{
    public CoreUnit coreUnit;

    public CoreUnitLostEvent(CoreUnit coreUnit)
    {
        this.coreUnit = coreUnit;
    }
}