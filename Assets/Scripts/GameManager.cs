using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 古希腊掌管敌人生成的神
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance;

    [SerializeField] private List<Enemy> enemyList;
    [SerializeField] private List<EnemySO> enemySOList;

    [SerializeField] private float timer;
    [SerializeField] private float timeForSpawn;
    [SerializeField] private float spawnRange;
    [SerializeField] private int spawnNumPerTime;
    [SerializeField] private Transform spawnCenter;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        SpawnEnemyPreFrame();
    }

    private void SpawnEnemyPreFrame()
    {
        if (enemySOList.Count == 0)
        {
            return;
        }

        if (timer < timeForSpawn)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            for (int i = 0; i < spawnNumPerTime; i++)
            {
                int randomIndex = Random.Range(0, enemySOList.Count);
                EnemySO enemySO = enemySOList[randomIndex];
                Vector3 offset = Random.insideUnitCircle.normalized * spawnRange;
                Enemy.CreateEnemyFactory(enemySO, transform.position + offset, Quaternion.identity);
            }
        }
    }

}
