using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using TMPro;
using UnityEngine.UI;
using QFramework;
using System;
using System.Security.Cryptography;

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

    #region 事件中心事件
    private void EventCenter_OnSpaceportShopCurBoughtChange(object sender, object e)
    {
        Debug.Log("接收事件 SpaceportShopCurBoughtChange 设置物品关闭");
        if (e is List<SpaceportShopProductInfo> infolist)
        {
            UpdateProduct(infolist[index]);
        }
    }
    #endregion

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void UpdateProduct(SpaceportShopProductInfo spaceportShopProductInfo)
    {
        // 如果已经买了就关闭子物体显示
        //Debug.Log("spaceportShopProductInfo.isBought: " + spaceportShopProductInfo.isBought);
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
        string langKey = this.GetUtility<LangUtility>().GetLanguageKey();

        // 设置全部显示
        nameText.text = spaceportShopProductInfo.unitSO.GetName(langKey);
        costText.text = spaceportShopProductInfo.cost.ToString();
        iconImg.sprite = spaceportShopProductInfo.unitSO.fullsizeSprite;

        //高级炮塔

        //耐久值:	25
        //射击间隔: 5
        //子弹伤害: 10
        //子弹速度: 60
        //子弹穿透: 1 -1

        //弹药消耗: 0.5 / s

        //弹药类型:

        // 描述: 高级炮塔

        // 寻找额外数据显示 通过unitso的名字搜索
        string desc = this.SendCommand(
            new GetProductDescCommand(
                spaceportShopProductInfo.unitSO,
                this.GetUtility<LangUtility>().GetLanguageKey()
                )
            );
        descText.SetText(desc);
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
            this.SendCommand(new BuyUnitFromShopCommand(index));
        });
    }


}

/// <summary>
/// 根据名字获取炮塔SO命令
/// </summary>
public class GetProductDescCommand : AbstractCommand<string>
{
    public UnitSO unit;
    public string langKey;

    public GetProductDescCommand(UnitSO unit, string langKey)
    {
        this.unit = unit;
        this.langKey = langKey;
    }

    protected override string OnExecute()
    {
        string desc = unit.GetDescription(langKey) + "\n" +
                      "\n" +
                      $"耐久值:\t{unit.maxHP}\n";

        // 寻找额外数据显示 通过unitso的名字搜索

        // 如果有添加炮塔数据
        var turretSO = this.GetModel<UnitExtraSOModel>().GetTurretSO(unit.name);
        if (turretSO != null)
        {
            desc += $"射击间隔:\t{turretSO.fireGap}s\n" +
                    $"索敌半径:\t{turretSO.radius}\n" +
                    $"子弹伤害:\t{turretSO.defaultProjectile?.damage}\n" +
                    $"子弹速度:\t{turretSO.defaultProjectile?.speed}\n" +
                    $"子弹穿透:\t{turretSO.defaultProjectile?.penetration - 1}\n" +
                    $"\n" +
                    $"弹药消耗:\t{turretSO.GetAmmoConsumeRate()} / s\n" +
                    $"\n" +
                    $"弹药类型:\t<size=40><sprite name=\"{turretSO.magazineInfos[0].magazineItem.name}\"></size>\n";
        }

        // 如果有护盾数据
        var shieldSO = this.GetModel<UnitExtraSOModel>().GetShieldSO(unit.name);
        if (shieldSO != null)
        {
            desc += $"护盾值:\t{shieldSO.shieldCapacity}\n" +
                    $"范围:\t{shieldSO.shieldRadius}\n" +
                    $"护盾恢复:\t{shieldSO.defaultRechargeNum} / {shieldSO.rechargeGap}s\n" +
                    $"重启时间:\t{shieldSO.restartTime}\n";
        }

        return desc;
    }
}