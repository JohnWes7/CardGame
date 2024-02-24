using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Johnwest.LogSprirtAni.Instance.LogSpriteLocal(transform.position, createItemSO.mainSprite);
            PlayerModel.Instance.GetInventory().AddItem(createItemSO, 1);
        }
    }
}
