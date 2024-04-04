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
        Debug.Log("刷新商店\n" + SpaceportShopModel.Instance.GetRefreshCost());
    }
}

/// <summary>
/// 购买单位命令
/// </summary>
public class BuyUnitCommand : AbstractCommand
{
    public int index;

    public BuyUnitCommand(int index)
    {
        this.index = index;
    }

    protected override void OnExecute()
    {
         // 购买逻辑

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