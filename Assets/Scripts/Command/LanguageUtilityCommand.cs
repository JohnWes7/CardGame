using System;
using System.Collections.Generic;
using QFramework;


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
