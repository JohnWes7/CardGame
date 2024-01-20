using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretStg
{
    void FireStretagePreFrame(ITurretWeapon turretWeapon);
}

public class TurretStg : ITurretStg
{
    /// <summary>
    /// 开火策略模式
    /// </summary>
    public void FireStretagePreFrame(ITurretWeapon turretWeapon)
    {


        //if (turretWeapon.Target != null)
        //{
        //    RaycastHit2D enemys = Physics2D.CircleCast(turretWeapon.Turret.transform.position, range, Vector2.zero, 0.0f, LayerMask.GetMask("Enemy"));
        //}


    }
}
