using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Johnwest
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance;

        private Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public ObjectPool GetPool(GameObject prefab, int initialSize = 0)
        {
            if (pools.ContainsKey(prefab))
            {
                return pools[prefab];
            }
            else
            {
                var newPool = new ObjectPool(prefab, initialSize, transform);
                pools.Add(prefab, newPool);
                return newPool;
            }
        }
    }


    public class ObjectPool
    {
        private GameObject prefab;
        private Queue<GameObject> pool = new Queue<GameObject>();
        private Transform poolParent; // 用于组织层级视图的父对象

        public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.poolParent = parent ?? new GameObject(prefab.name + " Pool").transform;

            for (int i = 0; i < initialSize; i++)
            {
                AddObjectToPool();
            }
        }

        private void AddObjectToPool()
        {
            var newObject = GameObject.Instantiate(prefab, poolParent);
            newObject.SetActive(false);
            pool.Enqueue(newObject);
        }

        public GameObject Get()
        {
            if (pool.Count == 0)
            {
                AddObjectToPool();
            }

            var obj = pool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true); // 确保从池中取出时激活对象
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false); // 禁用对象，以便它停止所有当前行为
            ResetObject(obj); // 可选：重置对象状态
            obj.transform.SetParent(poolParent); // 重新设置父对象，以保持层级视图的整洁
            pool.Enqueue(obj);
        }

        private void ResetObject(GameObject obj)
        {
            // 这里重置对象的状态，例如位置、旋转、速度等
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;

            // 如果有刚体则重置速度
            var rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

}



