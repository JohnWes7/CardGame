using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class LangUtility : IUtility
{
    public const string LANGUAGE_PLAYERPREFS_KEY = "lang";

    public string GetLanguageKey()
    {
        string key =  PlayerPrefs.GetString(LANGUAGE_PLAYERPREFS_KEY, null);
        if (string.IsNullOrEmpty(key))
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
        return key;
    }
}
