using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUnaimWeapon : TurretWeaponV2
{
    protected override bool CheckTakeAim()
    {
        return true;
    }

    protected override void RotateTurret(Vector3 dest, float deltaTime)
    {
        
    }
}
