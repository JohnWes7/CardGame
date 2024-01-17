using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private float speed;
    [SerializeField] private EnemySO data;

    public static Enemy CreateEnemyFactory(EnemySO data)
    {
        GameObject gameObject = Instantiate<GameObject>(data.prefab);

        Enemy enemy = gameObject.GetComponent<Enemy>();

        // 没有的话就安装一个
        enemy = enemy == null ? gameObject.AddComponent<Enemy>() : enemy;

        enemy.hp = data.maxHP;
        enemy.speed = data.speed;

        return enemy;
    }
}
