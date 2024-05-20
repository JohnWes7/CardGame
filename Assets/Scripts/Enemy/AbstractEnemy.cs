using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using QFramework;

public abstract class AbstractEnemy : MonoBehaviour, IController
{
    [SerializeField, ReadOnly] protected int curHp;
    [SerializeField, ReadOnly] protected int maxHP;
    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected Transform target;

    public int CurHP { get => curHp; set => curHp = value; }
    public EnemySO Data { get => enemySO; set => enemySO = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }

    public static AbstractEnemy CreateEnemyFactory(EnemySO data)
    {
        GameObject gameObject = Instantiate<GameObject>(data.prefab);

        AbstractEnemy enemy = gameObject.GetComponent<AbstractEnemy>();

        // 没有的话就安装一个
        enemy = enemy == null ? gameObject.AddComponent<AbstractEnemy>() : enemy;

        enemy.maxHP = data.maxHP;
        enemy.curHp = data.maxHP;

        return enemy;
    }

    public static AbstractEnemy CreateEnemyFactory(EnemySO data, Vector3 position, Quaternion rotation)
    {
        AbstractEnemy enemy = CreateEnemyFactory(data);
        enemy.transform.position = position;
        enemy.transform.rotation = rotation;

        return enemy;
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
