using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// 参数对象模式 的参数对象
    /// </summary>
    [System.Serializable]
    public class ProjectileCreationParams
    {
        public ProjectileSO projectileSO;
        public Transform target;
        public Vector3 originPos = Vector3.zero;
        public Vector2 direction = Vector2.zero;
        public object creater = null;

        public ProjectileCreationParams(ProjectileSO projectileSO, Transform tgt, Vector3 origin, Vector2 moveDir, object crt = null)
        {
            this.projectileSO = projectileSO;
            target = tgt;
            originPos = origin;
            direction = moveDir;
            creater = crt;
        }

        public ProjectileCreationParams(ProjectileSO projectileSO, Transform target, Vector3 originPos, object creater)
        {
            this.projectileSO = projectileSO;
            this.target = target;
            this.originPos = originPos;
            this.creater = creater;

            // 计算方向
            if (target)
            {
                Vector3 vector3 = target.position - originPos;
                direction = new Vector2(vector3.x, vector3.y);
            }
        }
    }


    [SerializeField] protected object creater;
    [SerializeField] protected ProjectileSO projectileSO;
    [SerializeField] protected Transform target;
    [SerializeField] protected Vector2 direction;

    public const float PROJECTILE_DURATION = 5f;

    public ProjectileSO ProjectileSO { get => projectileSO; set => projectileSO = value; }
    public Transform Target { get => target; set => target = value; }
    public object Creater { get => creater; }
    public Vector2 Direction { get => direction; set => direction = value; }

    /// <summary>
    /// 子弹生成工厂方法
    /// </summary>
    /// <param name="projectileSO"></param>
    /// <param name="projectileCreationParams"></param>
    /// <returns></returns>
    public static Projectile ProjectileCreateFactory(ProjectileCreationParams projectileCreationParams)
    {
        // 子弹使用对象池来创建
        //GameObject go = Instantiate<GameObject>(projectileSO.prefab);
        // 如果没有对象池则直接创建
        GameObject go = null;
        if (Johnwest.ObjectPoolManager.Instance)
        {
            go = Johnwest.ObjectPoolManager.Instance.GetPool(projectileCreationParams.projectileSO.prefab).Get();
        }
        if (!go)
        {
            go = Instantiate<GameObject>(projectileCreationParams.projectileSO.prefab);
        }
         

        Projectile projectile = go.GetComponent<Projectile>();
        projectile = projectile != null ? projectile : go.AddComponent<Projectile>();
        projectile.target = projectileCreationParams.target;
        projectile.projectileSO = projectileCreationParams.projectileSO;
        projectile.direction = projectileCreationParams.direction.normalized;

        // 设置子弹的位置和方向
        projectile.transform.SetPositionAndRotation(projectileCreationParams.originPos, Quaternion.LookRotation(projectileCreationParams.direction, Vector3.forward));

        // 调用子弹初始化函数
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
