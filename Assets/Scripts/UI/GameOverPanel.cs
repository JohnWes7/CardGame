using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : SingletonUIBase<GameOverPanel>, IController
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void OnReturnMainMenuButtonClick()
    {
        this.SendCommand(new AsyncLoadSceneCommand("StartMenu", this));
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
