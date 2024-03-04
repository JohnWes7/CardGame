using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamageInfo
{
    public int damageAmount;
    public object Creater;
    public Vector3 hitPosition;

    public DamageInfo(int damage, object Creater)
    {
        this.damageAmount = damage;
        this.Creater = Creater;
    }

    public int GetDamageAmount()
    {
        return damageAmount;
    }

    public override string ToString()
    {
        return $"damage: {damageAmount}, creater: {Creater}";
    }
}

public interface IBeDamage
{
    void BeDamage(DamageInfo projectile);
}
