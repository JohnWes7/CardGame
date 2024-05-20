using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class INFIronV2 : INFIron
{

    protected override void Update()
    {
        if (timer < creatTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;

            // 动画 之后可以包装为一个command
            transform.DOScale(Vector3.one * 1.1f, 0.1f)
            .OnComplete(() => {
                // Scale down the building after scaling up
                transform.DOScale(Vector3.one, 0.1f);
            });

            PlayerModel.Instance.GetInventory().AddItem(createItemSO, 1);
        }
    }
}
