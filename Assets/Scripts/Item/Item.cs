using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static Item CreateItemFactory(ItemSO itemSO)
    {
        GameObject go = Instantiate<GameObject>(itemSO.prefab);

        Item item = go.GetComponent<Item>();
        item = item != null ? item : go.AddComponent<Item>();

        //if (item == null)
        //{
        //    item = go.AddComponent<Item>();
        //}

        return item;
    }


    [SerializeField] private ItemSO itemSO;
}