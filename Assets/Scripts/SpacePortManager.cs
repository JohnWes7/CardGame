using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePortManager : MonoBehaviour
{
    private void Start()
    {
        PlayerModel.Instance.LoadLocalSave();
        EventCenter.Instance.TriggerEvent("ShipMementoLoad", this, PlayerModel.Instance.GetShipMemento());
    }
}
