using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

/// <summary>
/// 获取当前关卡的剩余时间
/// </summary>
public class GetStageModelTimerCommand : AbstractCommand
{
    public float mStageTimer;

    protected override void OnExecute()
    {
        mStageTimer = this.GetModel<StageModel>().GetStageTimer();
    }
}

/// <summary>
/// 根据stageindex 重置当前关卡信息
/// </summary>
public class ResetStageInfoCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<StageModel>().ResetStageInfo();
    }
}

/// <summary>
/// 获取stage index
/// </summary>
public class GetStageIndexCommand : AbstractCommand
{
    public int mStageIndex;

    protected override void OnExecute()
    {
        mStageIndex = this.GetModel<StageModel>().GetStageIndex();
    }
}