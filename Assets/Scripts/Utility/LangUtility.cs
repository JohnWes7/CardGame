using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

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

        //Debug.Log($"LangUtility: 当前语言key : {key}");
        return key;
    }

    public void SetLanguageKey(string key)
    {
        Debug.Log($"LangUtility: 尝试写入key : {key}");

        if (Enum.TryParse(key, out LanguageKeyEnum _))
        {
            PlayerPrefs.SetString(LANGUAGE_PLAYERPREFS_KEY, key);
            return;
        }

        Debug.LogError("尝试向语言字典中存入不能代表语言的可以 try insert a null key");
    }
}
