using System;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(fileName = "AllStageSO", menuName = "ScriptableObject/AllStageSO")]
public class AllStageSO : ScriptableObject
{
    [Foldout]
    public List<StageInfoSO> stageInfoSOs;

    public StageInfoSO GetStageInfoSO(int index)
    {
        if (index < 0 || index >= stageInfoSOs.Count)
        {
            throw new ArgumentOutOfRangeException("index", "index out of range");
        }
        return stageInfoSOs[index];
    }
}
