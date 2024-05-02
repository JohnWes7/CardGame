using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfoSO", menuName = "ScriptableObject/StageInfoSO")]
public class StageInfoSO : ScriptableObject
{
    /// <summary>
    /// 关卡持续时间
    /// </summary>
    public float timeLimit = 60f;
    /// <summary>
    /// 敌人初始时间间隔
    /// </summary>
    public float enemySpawnInterval = 1f;
    /// <summary>
    /// 每次刷新的敌人数量
    /// </summary>
    public int enemySpawnCount = 1;
    /// <summary>
    /// 敌人存在上限
    /// </summary>
    public int enemyMaxCount = 100;
    /// <summary>
    /// 会刷新的敌人列表
    /// </summary>
    public List<EnemySpawnProbability> enemyInfos;
}

[Serializable]
public class EnemySpawnProbability
{
    public EnemySO enemySO;
    public float probability;
}

// 如果之后需要刷特殊敌人，可以在这里加一个特殊敌人列表 extrainfo
