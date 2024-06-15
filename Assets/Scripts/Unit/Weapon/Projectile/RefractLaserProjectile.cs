using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UIElements;

public class RefractLaserProjectile : LaserProjectileBase
{
    public float searchRadius = 10f;
    public List<MonoInterface<IBeDamage>> childBeDamages = new();

    public override void DoDamage(AbstractTurret abstractTurret)
    {
        base.DoDamage(abstractTurret);

        // 剔除不能使用的
        childBeDamages.RemoveAll(beDamage =>
            beDamage.MonoBehaviour == null);

        // 遍历childBeDamages 并且对每一个IBeDamage执行伤害
        childBeDamages.ForEach(beDamage =>
        {
            beDamage.InterfaceObj.BeDamage(new DamageInfo(projectileSO.damage, abstractTurret));
        });
    }

    public override void LaserUpdate(AbstractTurret turret, float deltaTime)
    {
        target = turret.GetTarget();
        if (target == null) return;

        // 剔除掉已有目标是 null 或者 目标超出范围的 在childBeDamages中的物体
        childBeDamages.RemoveAll(beDamage =>
            beDamage.MonoBehaviour == null ||
            Vector2.Distance(beDamage.MonoBehaviour.transform.position, transform.position) > searchRadius);

        // 如果childBeDamages的数量 >= Projectile中的穿透值的数量，就不用继续寻找了
        if (childBeDamages.Count < projectileSO.penetration - 1)
        {
            // 寻找target周围 半径为searchRadius 并且layer是projectileso中指定的layer的物体
            RaycastHit2D[] results = Physics2D.CircleCastAll(target.transform.position,
                searchRadius,
                Vector2.zero,
                0f,
                projectileSO.targetLayer);

            // 遍历找到的物体
            foreach (RaycastHit2D result in results)
            {
                // 如果数量超过穿透值，就跳出循环
                if (childBeDamages.Count >= projectileSO.penetration - 1) break;

                // 如果找到的物体已经在childBeDamages中，就跳过
                if (childBeDamages.Exists(beDamage => beDamage.MonoBehaviour == result.collider.gameObject)) continue;
                // 如果找到的物体是自己 的target (主要目标)，就跳过
                if (result.collider.gameObject == target.gameObject) continue;

                // 如果找到的物体是IBeDamage类型的，就添加到childBeDamages中
                if (result.collider.TryGetComponent<IBeDamage>(out IBeDamage beDamage))
                    childBeDamages.Add(new MonoInterface<IBeDamage>(beDamage));
            }
        }

        // 设置lineRenderer的位置
        var command = new LaserLineRendererSetCommand()
        {
            lineRenderer = lineRenderer,
            positions = new List<Vector3>()
            {
                turret.GetProjectileCreatePos(),
                target.transform.position
            }
        };
        command.positions.AddRange(childBeDamages.ConvertAll(beDamage => beDamage.MonoBehaviour.transform.position));
        this.SendCommand(command);
    }
}

public class LaserLineRendererSetCommand : AbstractCommand
{
    public LineRenderer lineRenderer;
    public List<Vector3> positions;

    protected override void OnExecute()
    {
        if (lineRenderer == null) return;
        if (positions == null) return;

        lineRenderer.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++) lineRenderer.SetPosition(i, positions[i]);
    }
}