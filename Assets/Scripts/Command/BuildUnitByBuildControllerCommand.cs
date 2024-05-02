using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

/// <summary>
/// 依赖于buildcontroller的buildcommand 之后可以改为不依赖
/// </summary>
public class BuildUnitByBuildControllerCommand : AbstractCommand
{
    public ShipBuildController shipBuildController;

    public BuildUnitByBuildControllerCommand(ShipBuildController shipBuildController)
    {
        this.shipBuildController = shipBuildController;
    }

    protected override void OnExecute()
    {
        UnitSO curUnitSO = shipBuildController.GetCurBuildUnit();
        if (curUnitSO == null)
        {
            return;
        }

        // 检查unit inventory 里面是否有超过一个
        if (PlayerModel.Instance.GetPlayerUnitInventory().GetUnitInventory().GetValueOrDefault(curUnitSO, -1) > 0)
        {
            UnitObject unitObject = ShipBuildingState.BuildUnit(shipBuildController);
            if (unitObject != null)
            {
                PlayerModel.Instance.GetPlayerUnitInventory().RemoveUnit(curUnitSO, 1);
            }
        }

        if (PlayerModel.Instance.GetPlayerUnitInventory().GetUnitInventory().GetValueOrDefault(curUnitSO, -1) <= 0)
        {
            // 如果里面小于一个 那就需要改变unit为null
            shipBuildController.ChangeCurBuildUnit(null);
        }

    }
}

public class DemolitionUnitByBuildControllerCommand : AbstractCommand
{
    public ShipBuildController shipBuildController;

    public DemolitionUnitByBuildControllerCommand(ShipBuildController shipBuildController)
    {
        this.shipBuildController = shipBuildController;
    }

    protected override void OnExecute()
    {
        // 先判断是要从拆除还是退出
        // 如果执行了拆除就不退出
        if (ShipBuildingState.TryDeleteUnitOnMousePos(shipBuildController, out UnitObject unitObject))
        {
            Debug.Log($"拆除: {unitObject}");

            // 拆除成功返还unit到仓库
            // 返还拆除的unit
            PlayerModel.Instance.GetPlayerUnitInventory().AddUnit(unitObject.UnitSO, 1);
        }
        else
        {
            // 如果没有拆除成功 就退出选择
            shipBuildController.ChangeCurBuildUnit(null);
        }
    }
}
