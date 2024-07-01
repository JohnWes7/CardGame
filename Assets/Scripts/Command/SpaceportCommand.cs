using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 刷新商店命令
/// </summary>
public class RefreshShopCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SpaceportShopModel.Instance.RefreshShop();
        //Debug.Log("刷新商店\n" + SpaceportShopModel.Instance.GetRefreshCost());
    }
}


/// <summary>
/// 购买单位命令
/// </summary>
public class BuyUnitFromShopCommand : AbstractCommand
{
    public struct BuyUnitFromShopEvent
    {
        public SpaceportShopProductInfo info;
        public bool state;
    }

    public int index;

    public BuyUnitFromShopCommand(int index)
    {
        this.index = index;
    }

    protected override void OnExecute()
    {
        // 购买的info
        SpaceportShopProductInfo info = SpaceportShopModel.Instance.GetCurrentUnits()[index];

        // 购买逻辑
        Debug.Log("尝试购买单位" + index + " : " + info.unitSO);

        // 检查货币是否足够
        if (PlayerModel.Instance.GetCurrency() >= info.cost)
        {
            SpaceportShopModel.Instance.SetBoughtFlag(index); // 标记购买
            PlayerModel.Instance.GetPlayerUnitInventory().AddUnit(info.unitSO, 1);  // 添加到玩家单位库存
            PlayerModel.Instance.CostCurrency(info.cost);   // 扣除货币

            Debug.Log("购买成功" + index + " : " + info.unitSO);

            // 触发购买事件
            this.SendEvent(new BuyUnitFromShopEvent()
            {
                info = info,
                state = true
            });
        }
        else
        {
            this.SendEvent(new BuyUnitFromShopEvent()
            {
                info = info,
                state = false
            });
            Debug.Log("货币不足");
        }

    }
}

public class TryBuildUnitCommand : AbstractCommand
{
    public UnitSO unitSO;

    public TryBuildUnitCommand(UnitSO unitSO)
    {
        this.unitSO = unitSO;
    }
    
    protected override void OnExecute()
    {

    }
}

/// <summary>
/// 触发刷新商店事件
/// </summary>
public class SendRefreshShopCommand : AbstractCommand
{
    public struct RefreshShopEvent
    {
        public bool state; // 是否成功刷新商店
    }

    public RefreshShopEvent refreshShopEvent;

    public SendRefreshShopCommand(RefreshShopEvent refreshShopEvent)
    {
        this.refreshShopEvent = refreshShopEvent;
    }

    protected override void OnExecute()
    {
        this.SendEvent(refreshShopEvent);
    }
}