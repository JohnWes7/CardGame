using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPanel : SingletonUIBase<SettingPanel>, IController
{
    [SerializeField, ForceFill] private TMP_Dropdown langDropDown;
    [SerializeField, ForceFill] private TMP_Dropdown resolutionDropdown;

    protected override void Awake()
    {
        base.Awake();
        langDropDown.onValueChanged.AddListener(OnLanguageChange);
    }

    public override void OpenUI()
    {
        base.OpenUI();
    }

    public void OnEnable()
    {
        RefreshLanguageOption();
    }

    public void RefreshLanguageOption()
    {
        string languageKey = this.SendCommand(new GetLanguageStringKeyCommand());
        int value = 0;
        switch (languageKey)
        {
            case "zh":
                value = 0;
                break;
            case "en":
                value = 1;
                break;
        }

        langDropDown.SetValueWithoutNotify(value);
    }

    public void OnLanguageChange(int value)
    {
        string key = "";
        switch (value)
        {
            case 0:
                key = "zh";
                break;
            case 1:
                key = "en";
                break;
            default:
                key = "zh";
                break;
        }

        this.SendCommand(new ChangeLanguageCommand(key));
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}



