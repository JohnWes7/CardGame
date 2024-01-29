using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public readonly int maxHP;
    public float speed;
    public GameObject prefab;
    public float warningRange;
    public LayerMask warningLayer;

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
