using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CustomInspector;
using QFramework;

public class AutoSwitchInputAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IController
{
    [ReadOnly]
    [SerializeField] 
    private string originMapName;
    [FixedValues("UI", "Move", "Build", "")]
    [SerializeField]
    private string targetMapName;

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        originMapName = this.SendCommand(new GetCurrentActionMapNameCommand());
        this.SendCommand(new SwitchActionMapCommand(targetMapName));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.SendCommand(new SwitchActionMapCommand(originMapName));
    }
}

/// <summary>
/// 获取当前的action map的值 如果获取不到 则返回move
/// </summary>
public class GetCurrentActionMapNameCommand : AbstractCommand<string>
{
    protected override string OnExecute()
    {
        try
        {
            return PlayerControllerSingleton.Instance.currentActionMap.name;
        }
        catch (System.Exception e)
        {
            Debug.Log($"{e} GetCurrentActionMapNameCommand : PlayerControllerSingleton.Instance.currentActionMap.name 获取失败, 不同脚本的Awake和OnEnable之间是没有确定的先后关系的");
        }
        return "Move";
    }
}

public class SwitchActionMapCommand : AbstractCommand
{
    public string targetMapName;

    public SwitchActionMapCommand(string targetMapName)
    {
        this.targetMapName = targetMapName;
    }

    protected override void OnExecute()
    {
        // 如果是空字符串那么就不更改
        if (string.IsNullOrWhiteSpace(targetMapName))
        {
            return;
        }

        PlayerControllerSingleton.Instance.SwitchCurrentActionMap(targetMapName);
        Debug.Log($"now action map: {PlayerControllerSingleton.Instance.currentActionMap.name}");
    }
}