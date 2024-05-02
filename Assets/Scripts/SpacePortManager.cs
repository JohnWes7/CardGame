using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePortManager : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    private void Start()
    {
        PlayerModel.Instance.LoadLocalSave();
        EventCenter.Instance.TriggerEvent("ShipMementoLoad", this, PlayerModel.Instance.GetShipMemento());
        
        this.GetModel<RelicModel>();

        SpaceportShopModel.Instance.GenerateCurUnits();
    }
}
