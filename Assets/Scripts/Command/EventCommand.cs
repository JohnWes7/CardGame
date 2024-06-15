using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class SendShowUnitDetailEventCommand : AbstractCommand
{
    public struct ShowUnitDetailEvent
    {
        public UnitObject unitObject;
    }

    public UnitObject unitObject;

    public SendShowUnitDetailEventCommand(UnitObject unitObject)
    {
        this.unitObject = unitObject;
    }

    protected override void OnExecute()
    {
        this.SendEvent(new ShowUnitDetailEvent()
        {
            unitObject = unitObject
        });
    }
}

/// <summary>
/// 发送ESC面板打开事件
/// </summary>
public class SendEscPanelOpenEventCommand : AbstractCommand
{
    public struct EscPanelOpenEvent
    {
        
    }

    protected override void OnExecute()
    {
        this.SendEvent(new EscPanelOpenEvent());
    }
}

/// <summary>
/// 发送ESC面板关闭事件
/// </summary>
public class SendEscPanelCloseEventCommand : AbstractCommand
{
    public struct EscPanelCloseEvent
    {
        
    }

    protected override void OnExecute()
    {
        this.SendEvent(new EscPanelCloseEvent());
    }
}