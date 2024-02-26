using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class ShieldBody : MonoBehaviour, IBeDamage
{
    [SerializeField, ForceFill]
    private ShieldUnit shieldUnit;

    public void BeDamage(DamageInfo projectile)
    {
        
    }
}
