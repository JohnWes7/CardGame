using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine.Localization.Settings;


/// <summary>
/// 获取当前语言的Key
/// </summary>
public class GetLanguageStringKeyCommand : AbstractCommand<string>
{
    protected override string OnExecute()
    {
        return this.GetUtility<LangUtility>().GetLanguageKey();
    }
}

/// <summary>
/// 写入语言的Key
/// </summary>
public class ChangeLanguageCommand : AbstractCommand
{
    public string key;

    public ChangeLanguageCommand(string key)
    {
        this.key = key;
    }

    protected override void OnExecute()
    {
        this.GetUtility<LangUtility>().SetLanguageKey(key);
    }
}


public class GetLocalizationStringCommand : AbstractCommand<string>
{
    public string table;
    public string entry;

    public GetLocalizationStringCommand(string table, string entry)
    {
        this.table = table;
        this.entry = entry;
    }

    protected override string OnExecute()
    {
        var loadingResult = LocalizationSettings.StringDatabase.GetTableEntry(table, entry);
        return loadingResult.Entry.GetLocalizedString();
    }
}

