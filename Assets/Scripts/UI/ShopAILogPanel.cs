using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using Febucci.UI;
using QFramework;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ShopAILogPanel : MonoBehaviour, IController
{
    [SerializeField, ForceFill]
    LocalizeStringEvent textStringEvent;
    [SerializeField, ForceFill]
    private TypewriterByCharacter noteText;
    [SerializeField]
    public List<string> entryList = new();

    // 文本 entry
    public string thxEntry;     // 购买成功
    public string buyFailEntry; // 购买失败
    public string unlockEntry;  // 解锁单位
    public string refreshEntry; // 刷新商店 成功
    public string refreshFailEntry; // 刷新商店 失败

    public void OnEnable()
    {
        textStringEvent.SetEntry(entryList[0]);
        textStringEvent.OnUpdateString.AddListener(PlayTextAni);

        // 注册购买事件
        this.RegisterEvent<BuyUnitFromShopCommand.BuyUnitFromShopEvent>(OnBuyUnitFromShopEvent)
            .UnRegisterWhenDisabled(gameObject);

        // 注册刷新事件
        this.RegisterEvent<SendRefreshShopCommand.RefreshShopEvent>(OnRefreshShopEvent)
            .UnRegisterWhenDisabled(gameObject);
    }

    public void OnDisable()
    {
        textStringEvent.OnUpdateString.RemoveListener(PlayTextAni);
    }

    public void OnBuyUnitFromShopEvent(BuyUnitFromShopCommand.BuyUnitFromShopEvent args)
    {
        // 如果购买成功显示谢谢惠顾
        if (!args.state && !string.IsNullOrWhiteSpace(buyFailEntry))
        {
            textStringEvent.SetEntry(buyFailEntry);
            noteText.StartShowingText(true);    // 重新播放动画
        }
        // 不然显示资金不足
        else if (args.state && !string.IsNullOrWhiteSpace(thxEntry))
        {
            textStringEvent.SetEntry(thxEntry);
            noteText.StartShowingText(true);
        }
    }

    public void OnRefreshShopEvent(SendRefreshShopCommand.RefreshShopEvent args)
    {
        // 如果刷新成功显示刷新成功entry
        if (args.state && !string.IsNullOrWhiteSpace(refreshEntry))
        {
            textStringEvent.SetEntry(refreshEntry);
            noteText.StartShowingText(true);    // 重新播放动画
        }
        // 如果刷新失败显示刷新失败entry
        else if (!args.state && !string.IsNullOrWhiteSpace(refreshFailEntry))
        {
            textStringEvent.SetEntry(refreshFailEntry);
            noteText.StartShowingText(true);    // 重新播放动画
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }


    public void PlayTextAni(string args)
    {
        noteText.ShowText(args);
    }
}
