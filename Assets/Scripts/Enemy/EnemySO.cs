using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;


[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/EnemySO")]
public class EnemySO : ScriptableObject
{
    [System.Serializable]
    public class EnemyDropItemInfo
    {
        public ItemSO itemSO;
        [Header("inclusive")]
        public int maxNum;
        [Header("exclusive")]
        public int minNum;
    }

    public int maxHP;
    [AssetsOnly, Preview]
    public GameObject prefab;
    public float warningRange;
    public LayerMask warningLayer;

    [HorizontalLine("运动数据")]
    public float maxTurnRate = 60f; // 每秒最大转速度, 单位为度
    public float maxSpeed = 5f; // 最大速度

    // 额外数据因为每个移动的代码逻辑可能不一样 需要 立一个新的SO

    //[HorizontalLine("额外数据不一定有用")]
    //public float slowDownDistance = 10f; // 减速距离

    //public float maxAcceleration = 10f; // 最大施加的力
    //public float slowDamping = 0.01f;



    [Header("死亡掉落")]
    public List<EnemyDropItemInfo> enemyDropItemInfos;

    public Dictionary<ItemSO, int> GetSpecificDrop()
    {
        Dictionary<ItemSO, int> result = new Dictionary<ItemSO, int>();
        foreach (var item in enemyDropItemInfos)
        {
            int dropvalue = Random.Range(item.minNum, item.maxNum);
            result.Add(item.itemSO, dropvalue);
        }

        return result;
    }

    [ContextMenu("TestDrop")]
    public void TestDrop()
    {
        List<string> log = new List<string>();
        var result = GetSpecificDrop();
        foreach (var item in result)
        {
            log.Add($"{item.Key} : {item.Value}");
        }

        Debug.Log(string.Join("\n", log));
    }
}
