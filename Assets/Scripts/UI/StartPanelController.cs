using System;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class StartPanelController : UIBase, IController
{
    public void Awake()
    {
        //this.SendCommand(new SyncLocalizationWithLangKeyCommand(this));
        StartCoroutine(InitializeLocalization(() =>
        {
            this.GetUtility<LangUtility>().GetLanguageKey();
        }));
    }

    public IEnumerator InitializeLocalization(Action onComplete)
    {
        var initializationOperation = LocalizationSettings.InitializationOperation;
        yield return initializationOperation;
        if (initializationOperation.Status ==
            UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogError("LocalizationSettings initialization failed.");
        }
    }


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

    // 测试获取的locale和selected的两个是否会相等
    //private void TestEquals()
    //{
    //    // 获取目标语言的 Locale 对象
    //    Locale newLocale = LocalizationSettings.AvailableLocales.GetLocale("zh");
    //    Debug.Log(newLocale == LocalizationSettings.SelectedLocale); // true
    //    Debug.Log(newLocale.Equals(LocalizationSettings.SelectedLocale)); // true
    //}

}