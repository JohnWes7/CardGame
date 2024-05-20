using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;


public class UnitExtraSOModel : AbstractModel
{
    public Dictionary<string, TurretSO> turretSODict = new();
    public Dictionary<string, ShieldSO> shieldSODict = new();

    protected override void OnInit()
    {
        // 导入所有的turretSO
        TurretSO[] turretSOs = Resources.LoadAll<TurretSO>("Default/Weapon/TurretSO") ?? throw new ArgumentNullException($"Resources.LoadAll<TurretSO>(\"Default/Weapon/TurretSO\")");
        foreach (var turretSO in turretSOs)
        {
            turretSODict.Add(turretSO.name, turretSO);
        }

        // 导入所有的shieldSO
        ShieldSO[] shieldSOs = Resources.LoadAll<ShieldSO>("Default/Unit/ShieldSO") ?? throw new ArgumentNullException($"Resources.LoadAll<ShieldSO>(\"Default/Unit/ShieldSO\")");
        foreach (ShieldSO shieldSO in shieldSOs)
        {
            shieldSODict.Add(shieldSO.name, shieldSO);
        }
    }

    public TurretSO GetTurretSO(string turretName)
    {
        return turretSODict.GetValueOrDefault(turretName, null);
    }

    public ShieldSO GetShieldSO(string shieldName)
    {
        return shieldSODict.GetValueOrDefault(shieldName, null);
    }
}
