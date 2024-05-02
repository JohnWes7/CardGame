using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class StageModel : AbstractModel
{
    // 静态数据
    private AllStageSO allStageSO;

    // 保存的动态数据 显示计时器用
    private float stageTimer = 60f;
    private int stageIndex = 0;

    protected override void OnInit()
    {
        stageTimer = 60f;

        // 之后可以改为reskit加载
        Debug.Log("StageModel OnInit");
        allStageSO = Resources.Load<AllStageSO>("Default/Stage/AllStageSO");
    }

    public void ResetStageInfo()
    {
        stageTimer = GetCurStageTimeLimit();
    }

    public void SetStageIndex(int index)
    {
        stageIndex = index;
    }

    public int GetStageIndex()
    {
        return stageIndex;
    }

    public void SetStageTimer(float time)
    {
        stageTimer = time;
        EventCenter.Instance.TriggerEvent("BattleTimeCountDown", this, stageTimer);
    }

    public float GetStageTimer()
    {
        return stageTimer;
    }

    public StageInfoSO GetCurStage()
    {
        return allStageSO.stageInfoSOs[stageIndex];
    }

    public float GetCurStageTimeLimit()
    {
        return allStageSO.stageInfoSOs[stageIndex].timeLimit;
    }
}


