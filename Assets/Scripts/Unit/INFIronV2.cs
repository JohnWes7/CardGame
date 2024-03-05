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
}
