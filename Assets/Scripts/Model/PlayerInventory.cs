using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    public class InventoryEventArgs : EventArgs
    {
        public Dictionary<ItemSO, int> dict;
        public ItemSO changeItemSO;

        public InventoryEventArgs(Dictionary<ItemSO, int> dict, ItemSO changeItemSO)
        {
            this.dict = dict;
            this.changeItemSO = changeItemSO;
        }
    }

    //// 单例
    //private static PlayerInventory instance;

    public event EventHandler<InventoryEventArgs> OnInventoryChange;

    [SerializeField] private Dictionary<ItemSO, int> inventory = new Dictionary<ItemSO, int>();

    //public static PlayerInventory Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = new PlayerInventory();
    //        }
    //        return instance;
    //    }
    //    set => instance = value;
    //}

    public void AddItem(ItemSO itemType, int num)
    {
        if (itemType == null)
        {
            Johnwest.JWUniversalTool.LogWarningWithClassMethodName("item type need add which is null", System.Reflection.MethodBase.GetCurrentMethod());
            return;
        }

        if (num < 0)
        {
            CostItem(itemType, -num);
        }

        // 如果是已经有的item就只要添加
        if (inventory.ContainsKey(itemType))
        {
            inventory[itemType] += num;
            OnInventoryChange?.Invoke(this, new InventoryEventArgs(inventory, itemType));
            return;
        }

        // 没有就添加key
        inventory.Add(itemType, num);
        OnInventoryChange?.Invoke(this, new InventoryEventArgs(inventory, itemType));
    }

    public Dictionary<ItemSO, int> GetInventory()
    {
        return inventory;
    }

    public override string ToString()
    {
        List<string> result = new List<string>();
        foreach (var item in inventory)
        {
            result.Add($"{item.Key} : {item.Value}");
        }

        return $"total num: {inventory.Count}\n{string.Join("\n", result)}";
    }

    /// <summary>
    /// 判断还有没有这些资源 如果itemcostlist 是null 则返回true
    /// </summary>
    /// <param name="itemCostList"></param>
    /// <returns></returns>
    public bool HaveEnoughItems(List<UnitSO.ItemCost> itemCostList)
    {
        if (itemCostList == null)
        {
            return true;
        }

        foreach (var item in itemCostList)
        {
            if (!inventory.ContainsKey(item.itemSO))
            {
                return false;
            }

            if (inventory[item.itemSO] < item.cost)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 判断有没有这些资源并返回 如果itemcostlist 是null 则返回true,
    /// 如果返回false 该种item不够 还会out 出需求的差量
    /// </summary>
    /// <param name="itemCostList"></param>
    /// <param name="missingItemLiost"></param>
    /// <returns></returns>
    public bool HaveEnoughItems(List<UnitSO.ItemCost> itemCostList, out List<UnitSO.ItemCost> missingItemLiost)
    {
        bool result = HaveEnoughItems(itemCostList);
        if (result)
        {
            missingItemLiost = null;
            return result;
        }

        missingItemLiost = new List<UnitSO.ItemCost>();
        foreach (var item in itemCostList)
        {
            if (!inventory.ContainsKey(item.itemSO))
            {
                missingItemLiost.Add(item.Copy());
                continue;
            }

            if (inventory[item.itemSO] < item.cost)
            {
                missingItemLiost.Add(new UnitSO.ItemCost(item.itemSO, item.cost - inventory[item.itemSO]));
            }
        }

        return result;
    }

    /// <summary>
    /// 判断该种item是否大于等于一定数量
    /// </summary>
    /// <param name="unitSO"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public bool HaveEnoughItem(ItemSO itemSO, int num = 1)
    {
        if (itemSO == null)
        {
            return true;
        }

        // 如果有该项物品并且大于num才返回true
        if (inventory.TryGetValue(itemSO, out int value))
        {
            if (value >= num)
            {
                return true;
            }
        }
        return false;
    }

    public Dictionary<string, int> ToItemNameNumPairs()
    {
        Dictionary<string, int> itemNameNumPairs = new Dictionary<string, int>();
        foreach (var item in inventory)
        {
            itemNameNumPairs.Add(item.Key.name, item.Value);
        }

        return itemNameNumPairs;
    }

    public void LoadFromItemNameNumPairs(Dictionary<string, int> itemNameNumPairs)
    {
        Clear();
        foreach (var item in itemNameNumPairs)
        {
            ItemSO itemSO = ItemInfoModel.Instance.GetItem(item.Key);
            AddItem(itemSO, item.Value);
        }
    }

    public void CostItem(ItemSO itemSO, int num)
    {
        if (inventory.ContainsKey(itemSO))
        {
            inventory[itemSO] = Mathf.Clamp(inventory[itemSO] - num, 0, int.MaxValue);
            OnInventoryChange.Invoke(this, new InventoryEventArgs(inventory, itemSO));
            return;
        }

        Debug.LogError($"not this key in inventory {itemSO}");
    }

    public bool TryCostItem(ItemSO itemSO, int num = 1)
    {
        if (inventory.TryGetValue(itemSO, out int value))
        {
            if (value >= num)
            {
                inventory[itemSO] = Mathf.Clamp(inventory[itemSO] - num, 0, int.MaxValue);
                OnInventoryChange.Invoke(this, new InventoryEventArgs(inventory, itemSO));
                return true;
            }
        }
        return false;
    }

    public int GetItemNum(ItemSO itemSO)
    {
        if (inventory.ContainsKey(itemSO))
        {
            return inventory[itemSO];
        }
        return 0;
    }

    public void Clear()
    {
        inventory.Clear();
    }
}
