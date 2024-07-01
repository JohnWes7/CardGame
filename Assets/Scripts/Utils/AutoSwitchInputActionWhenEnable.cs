using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using QFramework;

public class AutoSwitchInputActionWhenEnable : MonoBehaviour, IController 
{
    [ReadOnly]
    [SerializeField]
    public string originMapName;
    [FixedValues("UI", "Move", "Build", "")]
    [SerializeField]
    public string targetMapName;

    public void OnEnable()
    {
        originMapName = this.SendCommand(new GetCurrentActionMapNameCommand());
        this.SendCommand(new SwitchActionMapCommand(targetMapName));
    }

    public void OnDisable()
    {
        this.SendCommand(new SwitchActionMapCommand(originMapName));
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
