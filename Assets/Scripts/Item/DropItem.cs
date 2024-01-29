using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IItemSO
{
    [SerializeField] protected ItemSO itemSo;
    [SerializeField] protected int num;

    // 添加过来拾取的无人机
    protected RecyclePlatformDrone pickUpDrone;

    public ItemSO ItemSO { get => itemSo; set => itemSo = value; }
    public int Num { get => num; set => num = value; }
    public RecyclePlatformDrone PickUpDrone { get => pickUpDrone; set => pickUpDrone = value; }

    public static DropItem DropItemFactory(ItemSO itemSO, int num, Vector3 posistion, Quaternion rotation)
    {
        GameObject go = Instantiate<GameObject>(itemSO.prefab);
        DropItem dropItem = go.GetComponent<DropItem>();
        dropItem = dropItem == null ? go.AddComponent<DropItem>() : dropItem;

        dropItem.itemSo = itemSO;
        dropItem.num = num;

        go.transform.position = posistion;
        go.transform.rotation = rotation;

        // 一定要改掉落的层不然获取不到
        go.layer = LayerMask.NameToLayer("DropItem");
        // 并且安装碰撞箱
        BoxCollider2D boxCollider2D = go.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;

        return dropItem;
    }
}
