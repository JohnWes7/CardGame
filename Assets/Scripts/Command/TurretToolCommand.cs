using QFramework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretCheckAimCommand : AbstractCommand<bool>
{
    public Transform target;
    public Transform turret;
    public float allowAngle = 5f;

    protected override bool OnExecute()
    {
        // 检查target是否是null 如果是的话就返回false
        if (target == null) return false;
        // 检查炮塔是否是null 如果是的话就返回 true
        if (turret == null) return true;

        Vector3 dest = target.position - turret.transform.position;
        Vector3 curAim = turret.transform.up;
        dest.z = 0;
        curAim.z = 0;

        // 如果角度小于允许角度就返回true 否则返回false
        return Mathf.Abs(Vector3.Angle(dest, curAim)) < allowAngle;
    }
}

public class TurretRotateTurretCommand : AbstractCommand
{
    public Vector3 dest;
    public float deltaTime;
    public Transform turretTransform;
    public TurretSO TurretSO;

    public TurretRotateTurretCommand(Vector3 dest, float deltaTime, Transform turretTransform, TurretSO turretSO)
    {
        this.dest = dest;
        this.deltaTime = deltaTime;
        this.turretTransform = turretTransform;
        TurretSO = turretSO;
    }

    protected override void OnExecute()
    {
        float angle = Mathf.Atan2(dest.y, dest.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        turretTransform.rotation =
            Quaternion.Slerp(turretTransform.rotation, rotation, TurretSO.rotateSpeed * deltaTime);
    }
}