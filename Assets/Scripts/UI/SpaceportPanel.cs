using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceportPanel : SingletonUIBase<SpaceportPanel>, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public override void OpenUI()
    {
        base.OpenUI();

        // 触发打开spcaeportPanel事件
        this.SendCommand(new TriggerSpaceportShopPanelEventCommand() { 
            sender = this,
            toState = true
        });
    }

    public override void CloseUI()
    {
        base.CloseUI();

        string originMapName = "Move";
        if (TryGetComponent<AutoSwitchInputActionWhenEnable>(out AutoSwitchInputActionWhenEnable component))
        {
            originMapName = component.originMapName;
        }

        // 触发关闭spcaeportPanel事件
        this.SendCommand(new TriggerSpaceportShopPanelEventCommand()
        {
            sender = this,
            toState = false,
            originMapName = originMapName
        });
    }
}

public class TriggerSpaceportShopPanelEventCommand : AbstractCommand
{
    public struct SpaceportPanelEvent
    {
        public object sender;
        public bool toState;
        public string originMapName;
    }

    public object sender;
    public bool toState;
    public string originMapName;

    protected override void OnExecute()
    {
        this.SendEvent<SpaceportPanelEvent>(new SpaceportPanelEvent() {
            sender = sender,
            toState = toState,
            originMapName = originMapName
        });
    }
}
