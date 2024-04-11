using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using TMPro;
using UnityEngine.UI;
using QFramework;
using System;

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

    private void Start()
    {
        EventCenter.Instance.AddEventListener("SpaceportShopCurBoughtChange", EventCenter_OnSpaceportShopCurBoughtChange);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener("SpaceportShopCurBoughtChange", EventCenter_OnSpaceportShopCurBoughtChange);
    }

    private void EventCenter_OnSpaceportShopCurBoughtChange(object sender, object e)
    {
        Debug.Log("接收事件 SpaceportShopCurBoughtChange");
        if (e is List<SpaceportShopProductInfo> infolist)
        {
            UpdateProduct(infolist[index]);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void UpdateProduct(SpaceportShopProductInfo spaceportShopProductInfo)
    {
        // 如果已经买了就关闭子物体显示
        Debug.Log("spaceportShopProductInfo.isBought: " + spaceportShopProductInfo.isBought);
        if (spaceportShopProductInfo.isBought)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        // 读取当前语言
        string langKey = PlayerPrefs.GetString("Language", "zh");

        // 设置全部显示
        nameText.text = spaceportShopProductInfo.unitSO.GetName(langKey);
        descText.text = spaceportShopProductInfo.unitSO.GetDescription(langKey);
        costText.text = spaceportShopProductInfo.cost.ToString();
        iconImg.sprite = spaceportShopProductInfo.unitSO.fullsizeSprite;
    }

    public void Init(SpaceportShopProductInfo spaceportShopProductInfo, int index)
    {
        if (spaceportShopProductInfo == null)
        {
            return;
        }

        // 保存index
        this.index = index;

        // 显示
        UpdateProduct(spaceportShopProductInfo);

        // 设置按钮事件
        buyBtn.onClick.AddListener(() =>
        {
            // 购买单位
            this.SendCommand(new BuyUnitCommand(index));
        });
    }

    
}
