using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CustomInspector;
using DG.Tweening;

public class RepairCenter : UnitObject, IShipUnit
{
    [SerializeField]
    private MonoInterface<IShipController> ship;

    [SerializeField]
    private float repairGap = 1f;
    [SerializeField]
    private float repairTimer = 0f;
    // 回复量
    [SerializeField]
    private int repairAmount = 5;
    [SerializeField]
    private int repairRadius = 10;
    [SerializeField]
    private GameObject droneFXPrefab;

    private void Update()
    {
        RepairUpdate(Time.deltaTime);
    }

    public void RepairUpdate(float deltaTime)
    {
        // 如果repairTimer小于repairGap，就继续累加
        if (repairTimer < repairGap)
        {
            repairTimer += deltaTime;
        }
        else
        {
            // 如果repairTimer大于repairGap，就开始回复
            repairTimer = 0;
            // 回复
            IBeRepairUnitObject target = FindRepairTarget();
            // Debug.Log(target);
            Repair(target);

        }
    }

    public void Repair(IBeRepairUnitObject target)
    {
        if (target != null)
        {
            // 生成一个特效
            GameObject fx = Instantiate(droneFXPrefab, transform.position, Quaternion.FromToRotation(transform.position, target.GetTransform().position), transform);
            // Debug.Log(transform.InverseTransformPoint(target.GetTransform().position));
            fx.transform.DOLocalMove(transform.InverseTransformPoint(target.GetTransform().position), 1).SetEase(Ease.InBack).OnComplete(() =>
            {
                try
                {
                    target.Repair(repairAmount);
                }
                catch (System.Exception)
                {
                    Debug.Log("target is null");
                }
                finally
                {
                    Destroy(fx);
                }
            });

        }
    }

    public IBeRepairUnitObject FindRepairTarget()
    {
        // 通过物理搜索半径内layer是Ship的物体
        var results = Physics2D.CircleCastAll(transform.position, repairRadius, Vector2.zero, 0, LayerMask.GetMask("Ship", "ShipOffline"));

        // 拿取所有IBeRepairUnitObject
        List<IBeRepairUnitObject> repairUnitObjects = new List<IBeRepairUnitObject>();
        foreach (var result in results)
        {
            // 如果找到了IBeRepairUnitObject，就加入到repairUnitObjects中
            if (result.collider != null)
            {
                IBeRepairUnitObject unitObj = result.collider.GetComponent<IBeRepairUnitObject>();
                if (unitObj != null)
                {
                    repairUnitObjects.Add(unitObj);
                }
            }
        }

        // 从repairUnitObjects中找到getHPScale最小的
        IBeRepairUnitObject target = null;
        foreach (var item in repairUnitObjects)
        {
            if (item.GetHPScale() < 1 && target == null)
            {
                target = item;
            }
            else
            {
                // 并且target的getHPScale小于1，且item的getHPScale大于target的getHPScale
                if (item.GetHPScale() < 1 && target.GetHPScale() > item.GetHPScale())
                {
                    target = item;
                }
            }
        }

        return target;
    }

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white; // 设置 Gizmos 颜色
        Gizmos.DrawWireSphere(transform.position, repairRadius); // 绘制表示射程范围的圆形
    }
}

public interface IBeRepairUnitObject
{
    public float GetHPScale();
    public void Repair(int amount);
    public Transform GetTransform();
}
