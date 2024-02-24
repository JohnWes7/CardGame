using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int bodyHP;
    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected Transform target;

    public int BodyHP { get => bodyHP; set => bodyHP = value; }
    public EnemySO Data { get => enemySO; set => enemySO = value; }

    public static Enemy CreateEnemyFactory(EnemySO data)
    {
        GameObject gameObject = Instantiate<GameObject>(data.prefab);

        Enemy enemy = gameObject.GetComponent<Enemy>();

        // 没有的话就安装一个
        enemy = enemy == null ? gameObject.AddComponent<Enemy>() : enemy;

        enemy.bodyHP = data.maxHP;

        return enemy;
    }

    public static Enemy CreateEnemyFactory(EnemySO data, Vector3 position, Quaternion rotation)
    {
        Enemy enemy = CreateEnemyFactory(data);
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        return enemy;
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
    }
}
