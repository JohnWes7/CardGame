using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public enum LanguageKeyEnum
{
    zh,
    en
}

public class LangUtility : IUtility
{
    public const string LANGUAGE_PLAYERPREFS_KEY = "lang";

    public string GetLanguageKey()
    {
        string key =  PlayerPrefs.GetString(LANGUAGE_PLAYERPREFS_KEY, null);
        //Debug.Log(key);
        
        // 安全检查 避免存入 或者输出不能正常代表的字符串
        if (string.IsNullOrEmpty(key) || !Enum.TryParse(key, out LanguageKeyEnum _))
        {
            SystemLanguage currentLanguage = Application.systemLanguage;

            switch (currentLanguage)
            {
                case SystemLanguage.English:
                    Debug.Log("English language is detected.");
                    key = "en";
                    break;

                // 中文全部使用zh
                case SystemLanguage.Chinese:
                    key = "zh";
                    break;
                case SystemLanguage.ChineseSimplified:
                    key = "zh";
                    break;
                case SystemLanguage.ChineseTraditional:
                    key = "zh";
                    break;

                //默认使用en
                default:
                    Debug.Log("Other language is detected: " + currentLanguage.ToString());
                    key = "en";
                    break;
            }
        }

        // 同步 localization
        SyncLocale(key);

        Debug.Log($"current language : {key}");
        Debug.Log($"SelectedLocale.LocaleName: {LocalizationSettings.SelectedLocale.LocaleName}");

        return key;
    }

    public void SetLanguageKey(string key)
    {
        Debug.Log($"LangUtility: 尝试写入key : {key}");

        if (Enum.TryParse(key, out LanguageKeyEnum _))
        {
            PlayerPrefs.SetString(LANGUAGE_PLAYERPREFS_KEY, key);
            ChangeLocale(key);
            return;
        }

        Debug.LogError("尝试向语言字典中存入不能代表语言的可以 try insert a null key");
    }

    private void ChangeLocale(string localeIdentifier)
    {
        // 获取目标语言的 Locale 对象
        Locale newLocale = LocalizationSettings.AvailableLocales.GetLocale(localeIdentifier);
        if (newLocale != null)
        {
            // 设置选定的 Locale
            LocalizationSettings.SelectedLocale = newLocale;
        }
        else
        {
            Debug.Log($"尝试获取locale失败: localeIdentifier : {localeIdentifier}");
        }
    }

    private void SyncLocale(string localeIdentifier)
    {
        // 获取目标语言的 Locale 对象
        Locale newLocale = LocalizationSettings.AvailableLocales.GetLocale(localeIdentifier);
        if (newLocale != null)
        {
            // 语言相等就不改
            if (LocalizationSettings.SelectedLocale == newLocale)
            {
                return;
            }

            // 设置选定的 Locale
            LocalizationSettings.SelectedLocale = newLocale;
        }
        else
        {
            Debug.Log($"尝试获取locale失败: localeIdentifier : {localeIdentifier}");
        }
    }

    
}

/// <summary>
/// 注意如果之后需要删除coroutine 可能会把这个删了
/// </summary>
public class SyncLocalizationWithLangKeyCommand : AbstractCommand
{
    public MonoBehaviour coroutineBehaviour;

    public SyncLocalizationWithLangKeyCommand(MonoBehaviour coroutineBehaviour)
    {
        this.coroutineBehaviour = coroutineBehaviour;
    }

    protected override void OnExecute()
    {
        coroutineBehaviour.StartCoroutine(InitializeLocalization(() =>
        {
            this.GetUtility<LangUtility>().GetLanguageKey();
        }));
    }

    public IEnumerator InitializeLocalization(Action onComplete)
    {
        var initializationOperation = LocalizationSettings.InitializationOperation;
        yield return initializationOperation;
        if (initializationOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogError("LocalizationSettings initialization failed.");
        }
    }
}