using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class StageModel : AbstractModel
{
    // 静态数据
    private AllStageSO allStageSO;

    // 保存的动态数据
    private float stageTimer = 60f; // 显示计时器用
    private int stageIndex = 0; // 当前关卡索引
    private bool isDone = false; // 是否完成

    protected override void OnInit()
    {
        Reset();
    }

    public void Reset()
    {
        stageTimer = 60f;
        stageIndex = 0;

        // 之后可以改为reskit加载
        Debug.Log("StageModel OnInit");
        allStageSO = Resources.Load<AllStageSO>("Default/Stage/AllStageSO");
    }

     
    #region Geter Seter
    public void ResetStageInfo()
    {
        stageTimer = GetCurStageTimeLimit();
        isDone = false;
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

    public bool IsDone()
    {
        return isDone;
    }

    public void SetDone(bool done)
    {
        isDone = done;
    }

    public AllStageSO GetAllStageSO()
    {
        return allStageSO;
    }

    public bool IsTimeOut()
    {
        return stageTimer <= 0;
    }
    #endregion
}


