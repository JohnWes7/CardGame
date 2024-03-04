using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class INFIronV2 : INFIron
{

    [SerializeField] protected List<float> offlineStateColorA = new List<float>();

    protected override void Update()
    {
        if (timer < creatTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            // Johnwest.LogSprirtAni.Instance.LogSpriteLocal(transform.position, createItemSO.mainSprite);
            transform.DOScale(Vector3.one * 1.1f, 0.1f)
            .OnComplete(() => {
                // Scale down the building after scaling up
                transform.DOScale(Vector3.one, 0.1f);
            });
            PlayerModel.Instance.GetInventory().AddItem(createItemSO, 1);
        }
    }

    public override void SetState(bool value)
    {
        base.SetState(value);
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
                color.a /= 2;
                spriteRenderer.color = color;
            }
        }
        else
        {
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
