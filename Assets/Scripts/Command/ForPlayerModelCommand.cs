using QFramework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 尝试扣去货币指令
/// </summary>
public class TryCostCurrencyCommand : AbstractCommand
{
    public int cost;
    public bool isCost;

    public TryCostCurrencyCommand(int cost)
    {
        this.cost = cost;
    }

    protected override void OnExecute()
    {
        if (PlayerModel.Instance.GetCurrency() >= cost)
        {
            PlayerModel.Instance.CostCurrency(cost);
            isCost = true;
            return;
        }

        isCost = false;
    }
}

/// <summary>
/// 从player model中获取已经解锁的单位
/// </summary>
public class GetAddtionalUnitCommand : AbstractCommand
{
    public List<UnitSO> unitSOList;

    protected override void OnExecute()
    {
        unitSOList = PlayerModel.Instance.GetTechRelicInventory().GetUnlockUnits();
    }
}

/// <summary>
/// 回收一个dropitem指令
/// </summary>
public class ReceiveDropItemCommand : AbstractCommand
{
    DropItem dropItem;

    public ReceiveDropItemCommand(DropItem dropItem)
    {
        this.dropItem = dropItem;
    }

    protected override void OnExecute()
    {
        // 如果改成货币 就需要判断是不是货币
        if (dropItem.ItemSO.type == ItemSO.Type.Currency)
        {
            Debug.Log($"receive drop currency: {dropItem.ItemSO} {dropItem.Num}");
            PlayerModel.Instance.AddCurrency(dropItem.Num);
            Object.Destroy(dropItem.gameObject);
            return;
        }

        Debug.Log($"receive drop item: {dropItem.ItemSO} {dropItem.Num}");
        PlayerModel.Instance.GetInventory()?.AddItem(dropItem.ItemSO, dropItem.Num);
        Object.Destroy(dropItem.gameObject);
    }
}

public class ShipSaveMemetoCommand : AbstractCommand
{
    public ShipController shipController;

    public ShipSaveMemetoCommand(ShipController shipController)
    {
        this.shipController = shipController ?? throw new System.ArgumentNullException(nameof(shipController));
    }

    protected override void OnExecute()
    {
        PlayerModel.Instance.SetShipMemento(shipController);
    }
}