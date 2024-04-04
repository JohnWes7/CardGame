using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using TMPro;
using UnityEngine.UI;
using QFramework;

public class SpaceportProduct : MonoBehaviour, IController
{
    [ForceFill]
    public TextMeshProUGUI nameText;
    [ForceFill]
    public TextMeshProUGUI costText;
    [ForceFill]
    public TextMeshProUGUI descText;
    [ForceFill]
    public Image iconImg;
    [ForceFill]
    public Button buyBtn;
    [ReadOnly]
    public int index;

    public IArchitecture GetArchitecture()
    {
        return SpaceportArchitecture.Interface;
    }

    public void Init(SpaceportShopProductInfo spaceportShopProductInfo, int index)
    {
        // 保存index
        this.index = index;

        // 读取当前语言
        string langKey = PlayerPrefs.GetString("Language", "zh");

        // 设置全部显示
        nameText.text = spaceportShopProductInfo.unitSO.GetName(langKey);
        descText.text = spaceportShopProductInfo.unitSO.GetDescription(langKey);
        costText.text = spaceportShopProductInfo.cost.ToString();
        iconImg.sprite = spaceportShopProductInfo.unitSO.fullsizeSprite;

        // 设置按钮事件
        buyBtn.onClick.AddListener(() =>
        {
            // 购买单位
            this.SendCommand(new BuyUnitCommand(index));
        });
    }
}
