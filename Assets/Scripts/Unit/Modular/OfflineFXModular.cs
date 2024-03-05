using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class OfflineFXModular : MonoBehaviour, IOfflineFXModular
{
    [SerializeField, ReadOnly]
    private List<float> offlineStateColorA = new List<float>();
    // a值变化的系数
    [SerializeField]
    private float aChangeFactor = 0.5f;

    public void SetState(bool value)
    {
        if (!value)
        {
            // 找到包括自己在内的所有spriteRenderer
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

            // 把所有spriteRenderer的color的a值保存到offlineStateColorA
            offlineStateColorA.Clear();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                offlineStateColorA.Add(spriteRenderer.color.a);
            }

            // 把所有spriteRenderer的color的a值设为原来的一半
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a *= aChangeFactor;
                spriteRenderer.color = color;
            }
        }
        else
        {
            // 如果之前没有保存过offlineStateColorA则返回
            if (offlineStateColorA.Count == 0)
            {
                return;
            }

            // 找到包括自己在内的所有spriteRenderer
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);

            // 把所有spriteRenderer的color的a值设为offlineStateColorA
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Color color = spriteRenderers[i].color;
                color.a = offlineStateColorA[i];
                spriteRenderers[i].color = color;
            }
        }
    }
}

public interface IOfflineFXModular
{
    void SetState(bool value);
}
