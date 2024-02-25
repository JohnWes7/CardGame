using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected UnitObject creater;
    [SerializeField] protected ProjectileSO projectileSO;
    [SerializeField] protected Transform target;

    public const float PROJECTILE_DURATION = 5f;

    public ProjectileSO ProjectileSO { get => projectileSO; set => projectileSO = value; }
    public Transform Target { get => target; set => target = value; }

    public static Projectile ProjectileCreateFactory(ProjectileSO projectileSO, Transform target)
    {
        return ProjectileCreateFactory(projectileSO, target, Vector3.zero);
    }

    public static Projectile ProjectileCreateFactory(ProjectileSO projectileSO, Transform target, Vector3 originPos, UnitObject creater = null)
    {
        // 子弹使用对象池来创建
        //GameObject go = Instantiate<GameObject>(projectileSO.prefab);
        GameObject go = Johnwest.ObjectPoolManager.Instance.GetPool(projectileSO.prefab).Get();

        Projectile projectile = go.GetComponent<Projectile>();
        projectile = projectile != null ? projectile : go.AddComponent<Projectile>();
        projectile.target = target;
        projectile.projectileSO = projectileSO;

        projectile.transform.position = originPos;

        projectile.Initialize();

        return projectile;
    }

    /// <summary>
    /// 初始化函数
    /// 每当ProjectileCreateFactory()创造出一个子弹的时候自动在结尾调用一次
    /// </summary>
    public virtual void Initialize()
    {

    }

    public virtual void Destroy()
    {
        // 销毁的时候重新加入池子
        Johnwest.ObjectPoolManager.Instance.GetPool(projectileSO.prefab).ReturnToPool(gameObject);
    }
}
