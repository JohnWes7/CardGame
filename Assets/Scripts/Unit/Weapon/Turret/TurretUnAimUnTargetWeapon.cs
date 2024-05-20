using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

/// <summary>
/// 没有目标且不需要旋转炮塔的塔 布雷器
/// </summary>
public class TurretUnAimUnTargetWeapon : AbstractTurret
{
    [SerializeField, ReadOnly] private float timer;

    // 逻辑组件
    [HorizontalLine("Stg")]
    [SerializeField, ReadOnly] private MonoInterface<ITurretFireStg> turretFireStg;


    protected override void Awake()
    {
        base.Awake();
        timer = 0;
        turretFireStg.InterfaceObj = GetComponent<ITurretFireStg>();
    }

    private void Update()
    {
        FireStrategyPreFrame(Time.deltaTime);
    }

    private void FireStrategyPreFrame(float deltaTime)
    {
        if (timer < turretSO.fireGap)
        {
            timer += deltaTime;
            return;
        }

        // 获取弹药重置时间
        ProjectileSO projectileSO = base.GetProjectile();
        timer = 0;

        if (projectileSO == null)
        {
            Debug.Log($"无法获取 弹药 TurretUnAimUnTargetWeapon unit: {unitSO.name}");
            return;
        }

        turretFireStg.InterfaceObj.FireStg(this, projectileSO);
    }


    public override Transform GetTarget()
    {
        return null;
    }

    public override Vector2 GetFireDir()
    {
        return transform.up;
    }

    public override Vector3 GetProjectileCreatePos()
    {
        return transform.position;
    }
}
