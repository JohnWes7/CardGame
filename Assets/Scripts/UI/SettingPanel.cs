using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingPanel : SingletonUIBase<SettingPanel>, IController
{
    [SerializeField, ForceFill] private TMP_Dropdown langDropDown;
    [SerializeField, ForceFill] private TMP_Dropdown resolutionDropdown;

    protected override void Awake()
    {
        base.Awake();
        langDropDown.onValueChanged.AddListener(OnLanguageChange);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
    }

    public void OnEnable()
    {
        RefreshLanguageOption();
        RefreshResolution();
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

    public void RefreshResolution()
    {
        // 获取系统支持的所有分辨率
        Resolution[] resolutions = Screen.resolutions;

        // 清除 Dropdown 现有选项
        resolutionDropdown.ClearOptions();

        // 创建一个新的列表来存储分辨率的字符串表示
        List<string> options = new List<string>();

        // 当前分辨率索引
        int currentResolutionIndex = 0;

        // 遍历所有分辨率并添加到选项列表中
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].ToString();
            options.Add(option);

            // 检查当前分辨率
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // 将选项添加到 Dropdown
        resolutionDropdown.AddOptions(options);

        // 设置 Dropdown 的默认选项为当前屏幕分辨率
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
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

    public void OnResolutionChange(int index)
    {
        // 获取用户选择的分辨率
        Resolution resolution = Screen.resolutions[index];
        Debug.Log($"Change Resolution to: {resolution}");

        // 设置屏幕分辨率
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}



