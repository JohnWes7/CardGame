using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class DisableUnitObjectCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent(new SetUnitObjetEnableEvent() {value = false});
    }
}