using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回收平台 负责收集敌人死亡后掉落的物资
/// </summary>
public class RecyclePlatform : UnitObject, IShipUnit
{
    [SerializeField] private MonoInterface<IShipController> shipController;

    // 之后可以改为SO
    [SerializeField] private float scanRange;
    [SerializeField] private List<RecyclePlatformDrone> idleDronesList;
    [SerializeField] private List<RecyclePlatformDrone> workingDronesList;

    // 运行逻辑:
    /**
     * 如果无人机还有空余 -> 扫描周围是否有可打捞的物品 -> 有则释放无人机前往打捞 直到送出所有的无人机
     * 如果没有无人机空余就什么也不干
     */
    public IShipController GetShip()
    {
        return shipController.InterfaceObj;
    }

    public void SetShip(IShipController sc)
    {
        shipController.InterfaceObj = sc;
    }

    private void Update()
    {
        if (HaveIdleDrone(out RecyclePlatformDrone idleDrone))
        {
            // 扫描
            var target = ScanDropItem();
            if (target != null)
            {
                idleDrone.SetTarget(target, this);
                idleDrone.transform.SetParent(null);
                idleDronesList.Remove(idleDrone);
                workingDronesList.Add(idleDrone);
            }
        }
    }

    public bool HaveIdleDrone(out RecyclePlatformDrone idleDrone)
    {
        idleDrone = null;
        if (idleDronesList.Count > 0)
        {
            idleDrone = idleDronesList[0];
            return true;
        }
        return false;
    }

    public DropItem ScanDropItem()
    {
        // 会有两个无人机去回收一个物体
        var hitinfo = Physics2D.CircleCast(transform.position, scanRange, Vector2.zero, 0f, LayerMask.GetMask("DropItem"));
        if (hitinfo.transform != null)
        {
            DropItem dropItem = hitinfo.transform.GetComponent<DropItem>();
            if (dropItem != null && dropItem.Pickuper == null)
            {
                return dropItem;
            }
        }
        return null;
    }

    public void SetDroneParent(RecyclePlatformDrone drone)
    {
        drone.transform.SetParent(this.transform);
    }

    public void ReceiveDropItem(DropItem dropItem)
    {
        // 改用command
        this.SendCommand(new ReceiveDropItemCommand(dropItem));
    }

    public void SetDroneIdel(RecyclePlatformDrone drone)
    {
        drone.transform.SetParent(transform);
        workingDronesList.Remove(drone);
        idleDronesList.Add(drone);
    }


    private void OnDestroy()
    {
        foreach (var item in idleDronesList)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
            
        }

        foreach (var item in workingDronesList)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

}