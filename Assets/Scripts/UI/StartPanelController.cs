using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelController : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
    

    public void OnStartButtonClick()
    {
        Debug.Log("游戏开始");
        this.SendCommand(new AsyncLoadSceneCommand("SpacePort", this));
    }
}

public class StartGameAndResetModelCommand : AbstractCommand
{
    public int initCurrency = 50;

    protected override void OnExecute()
    {
        // 重置player model
        
    }
}
