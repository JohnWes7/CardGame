using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipBuildingState
{
    public void Update(ShipBuildController sbc);
    public void QuitBuild(ShipBuildController sbc);
    public void StartBuild(ShipBuildController sbc);
    public void ChangeIndex(ShipBuildController sbc, int index);
    public void Init(ShipBuildController sbc);
}
