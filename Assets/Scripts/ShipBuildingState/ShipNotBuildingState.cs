using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipNotBuildingState : IShipBuildingState
{
    public ShipNotBuildingState(ShipBuildController sbc)
    {
        Init(sbc);
    }

    public void ChangeIndex(ShipBuildController sbc, int index)
    {
        // do nothing
    }

    public void Init(ShipBuildController sbc)
    {
        // 进入该状态的时候调用
        if (sbc.PrefabShadow != null)
        {
            sbc.PrefabShadow.gameObject.SetActive(false);
        }

        // 退出建造建造方向复原
        sbc.BuildDir = Dir.up;
        sbc.Sc.InterfaceObj.SetAllFGridNodeBackGroundActive(false);
    }

    public void QuitBuild(ShipBuildController sbc)
    {
        // 已经是非建造状态了 do nothing
    }

    public void StartBuild(ShipBuildController sbc)
    {
        // 非建造状态转换到建造状态
        //sbc.State = new ShipBuildingState(sbc);
    }

    public void Update(ShipBuildController sbc)
    {
        // do nothing
    }


}
