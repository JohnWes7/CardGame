using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CustomInspector;
using QFramework;
using System;

/// <summary>
/// 古希腊掌管敌人生成的神
/// </summary>
public class GameManager : MonoBehaviour, IController
{
    private static GameManager instance;

    public static GameManager Instance => instance;

    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnRange;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private Transform initTarget;


    private void Awake()
    {
        instance = this;
        spawnTimer = 0f;

        Debug.Log("开始初始化");
        PlayerModel.Instance.LoadLocalSave();

        // 重置关卡
        this.SendCommand<ResetStageInfoCommand>();
    }

    private void Start()
    {
        EventCenter.Instance.TriggerEvent("ShipMementoLoad", this, PlayerModel.Instance.GetShipMemento());
    }

    private void Update()
    {
        CheckTimeOut();
        SpawnEnemyPreFrame();
    }

    /// <summary>
    /// 检查时间是否结束 如果结束则跳到下一关的商店如果 没有下一关则表示游戏结束
    /// </summary>
    private void CheckTimeOut()
    {
        GetStageModelTimerCommand getStageModelTimerCommand = new();
        this.SendCommand(getStageModelTimerCommand);

        if (getStageModelTimerCommand.mStageTimer <= 0)
        {
            // 关卡索引+1
            StageModel stageModel = this.GetModel<StageModel>();
            int stageindex = stageModel.GetStageIndex() + 1;

            // 如果没有下一关则表示游戏结束
            int stageCount = stageModel.GetAllStageSO().stageInfoSOs.Count;
            if (stageindex >= stageCount)
            {
                // 关闭player
                EventCenter.Instance.TriggerEvent("DisabalePlayer", this, null);
                
                // 显示面板

            }


            AsyncLoadSceneCommand asyncLoadSceneCommand = new("SpacePort", this);
        }
    }

    private void SpawnEnemyPreFrame()
    {
        StageModel stageModel = this.GetModel<StageModel>();

        // 减少关卡剩余时间
        if (stageModel.GetStageTimer() > 0)
        {
            stageModel.SetStageTimer(Mathf.Clamp(stageModel.GetStageTimer() - Time.deltaTime, 0, stageModel.GetCurStageTimeLimit()));
        }

        // 检查刷新敌人的信息
        StageInfoSO stageInfoSO = stageModel.GetCurStage();
        if (stageInfoSO.enemyInfos.Count == 0)
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= stageInfoSO.enemySpawnInterval)
        {
            spawnTimer = 0;
            for (int i = 0; i < stageInfoSO.enemySpawnCount; i++)
            {
                // 获取随机敌人
                EnemySO createEnemy = GetRandomEnemy(stageInfoSO.enemyInfos);

                // 随机生成位置
                Vector3 offset = UnityEngine.Random.insideUnitCircle.normalized * spawnRange;
                Vector3 center = spawnCenter.position;
                center.z = 0;

                // 创建敌人
                if (createEnemy != null)
                {
                    Enemy enemy = Enemy.CreateEnemyFactory(createEnemy, center + offset, Quaternion.identity);
                    if (initTarget)
                    {
                        enemy.SetTarget(initTarget);
                    }
                }

            }
        }


    }

    private EnemySO GetRandomEnemy(List<EnemySpawnProbability> enemyInfoSO)
    {
        float randomIndex = UnityEngine.Random.Range(0f, 1f);
        float sum = 0;
        foreach (var item in enemyInfoSO)
        {
            if (randomIndex >= sum && randomIndex < sum + item.probability)
            {
                return item.enemySO;
            }
        }

        // 返回最后一个
        return enemyInfoSO[enemyInfoSO.Count - 1].enemySO;
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}