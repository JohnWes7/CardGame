using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelController : UIBase, IController
{
    public override void Initialize(object args = null)
    {

    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void OnSettingsButtonClick()
    {
        SettingPanel.Instance.OpenUI();
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
    

    public void OnStartButtonClick()
    {
        Debug.Log("游戏开始");
        this.SendCommand(new StartGameResetModelByJsonCommand());
        this.SendCommand(new AsyncLoadSceneCommand("SpacePort", this));
    }

}