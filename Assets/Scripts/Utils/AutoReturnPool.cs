using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturnPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float delay = 1f;

    public void Initialize(GameObject prefab, float delay)
    {
        this.prefab = prefab;
        this.delay = delay;

        StartCoroutine(ReturnPool());
    }

    public IEnumerator ReturnPool()
    {
        yield return new WaitForSeconds(delay);
        Johnwest.ObjectPoolManager.Instance.GetPool(prefab).ReturnToPool(gameObject);
        Destroy(this);
    }
}
