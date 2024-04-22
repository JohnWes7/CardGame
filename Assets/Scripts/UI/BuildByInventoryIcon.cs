using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;
using QFramework;

public class BuildByInventoryIcon : MonoBehaviour, IController
{
    [ForceFill]
    public Image unitIcon;
    [ForceFill]
    public TextMeshProUGUI numText;
    [ForceFill]
    public Button button;
    [ForceFill]
    public TextMeshProUGUI nameText;
    public UnitSO unitSO;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void Refresh(UnitSO unitSO, int num)
    {
        // 保存单位数据
        this.unitSO = unitSO;

        // 设置图标
        unitIcon.sprite = unitSO.fullsizeSprite;

        // 设置名字
        // 读取当前语言
        string langKey = PlayerPrefs.GetString("Language", "zh");
        nameText.text = unitSO.GetName(langKey);

        // 设置数量
        numText.text = num.ToString("D3");
    }

    public void OnClick()
    {
        // 执行更改建造单位的命令
        this.SendCommand(new ChangeBuildUnitCommand(unitSO));
    }
}

public class ChangeBuildUnitCommand : AbstractCommand
{
    private UnitSO unitSO;

    public ChangeBuildUnitCommand(UnitSO unitSO)
    {
        this.unitSO = unitSO;
    }

    protected override void OnExecute()
    {
        // 通过事件中心更改
        EventCenter.Instance.TriggerEvent("BuildUnitChange", this, unitSO);
    }
}
