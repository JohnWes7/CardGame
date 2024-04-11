using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;

public class BuildByInventoryIcon : MonoBehaviour
{
    [ForceFill]
    public Image unitIcon;
    [ForceFill]
    public TextMeshProUGUI numText;
    public UnitSO unitSO;

    public void Refresh(UnitSO unitSO, int num)
    {
        // 保存单位数据
        this.unitSO = unitSO;

        // 设置图标
        unitIcon.sprite = unitSO.fullsizeSprite;

        // 设置数量
        numText.text = num.ToString("D3");
    }
}
