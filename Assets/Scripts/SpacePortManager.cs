using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePortManager : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    private void Awake()
    {
        // 检查并初始化 playermodel
        this.SendCommand<CheckPlayerModelLoadLocalSaveCommand>();

        // 注册事件 在unit被建造的时候会直接停止
        this.RegisterEvent<BuildUnitEvent>(e =>
        {
            Debug.Log($"停止了该 object 的脚本 stop unitobject script in : {e.unitObject.name}");
            e.unitObject.enabled = false;
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void Start()
    {
        // 加载飞船
        EventCenter.Instance.TriggerEvent("ShipMementoLoad", this, PlayerModel.Instance.GetShipMemento());

        // 停止update
        this.SendCommand<DisableUnitObjectCommand>();

        // 刷新商店
        SpaceportShopModel.Instance.GenerateCurUnits();
    }
}
