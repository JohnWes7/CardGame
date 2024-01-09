using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShipUnit
{
    public void SetShip(IShipController sc);
    public IShipController GetShip();
}
