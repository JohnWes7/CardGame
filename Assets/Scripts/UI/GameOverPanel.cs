using CustomInspector;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class GameOverPanel : SingletonUIBase<GameOverPanel>, IController
{
    [SerializeField, ForceFill]
    private LocalizeStringEvent titleStringEvent;

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

    private void ChangeTitle(string key)
    {
        if (titleStringEvent != null)
        {
            titleStringEvent.StringReference.TableEntryReference = key;
            titleStringEvent.RefreshString();
        }
    }

    public void ChangeGameLostTitle()
    {
        ChangeTitle("ui_failretry");
    }
}
