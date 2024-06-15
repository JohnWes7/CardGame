using System;
using System.Collections.Generic;
using CustomInspector;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ESCPanel : SingletonUIBase<ESCPanel>, IController
{
    [SerializeField, ReadOnly] private string lastMapName;
    [SerializeField, ForceFill] private GameObject settingPanel;

    public override void OpenUI()
    {
        base.OpenUI();

        // 保存 之前的map 并且切换到UI map
        lastMapName = PlayerControllerSingleton.Instance.currentActionMap.name;
        PlayerControllerSingleton.Instance.SwitchCurrentActionMap("UI");

        // 触发打开esc面板事件事件
        this.SendCommand(new SendEscPanelOpenEventCommand());
    }

    public override void CloseUI()
    {
        base.CloseUI();

        // 如果之前打开的时候保存过map 则切换回去
        if (string.IsNullOrWhiteSpace(lastMapName)) return;
        PlayerControllerSingleton.Instance.SwitchCurrentActionMap(lastMapName);
        Debug.Log(lastMapName = PlayerControllerSingleton.Instance.currentActionMap.name);
        lastMapName = null;

        // 触发关闭esc面板事件事件
        this.SendCommand(new SendEscPanelCloseEventCommand());
    }

    public void OnBackButtonClick()
    {
        CloseUI();
    }

    public void OnMainMenuButtonClick()
    {
        this.SendCommand(new AsyncLoadSceneCommand("StartMenu", this));
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    /// <summary>
    /// 由 player input 出发的ESC事件
    /// 要求 如果setting panel还在的话会先关闭setting panel
    /// </summary>
    /// <param name="context"></param>
    public void PlayerInputOn_OnESC(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 如果setting panel还在的话会先关闭setting panel
        if (settingPanel.activeInHierarchy)
        {
            settingPanel.SetActive(false);
            return;
        }

        if (isActiveAndEnabled)
        {
            CloseUI();
            return;
        }
        OpenUI();
    }
}