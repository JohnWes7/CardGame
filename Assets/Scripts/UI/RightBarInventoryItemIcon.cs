using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CustomInspector;

public class RightBarInventoryItemIcon : MonoBehaviour
{
    [SerializeField, ReadOnly] private ItemSO itemSO;
    [SerializeField, ReadOnly] private int num;
    [SerializeField, ForceFill] private Image itemImage;
    [SerializeField, ForceFill] private TextMeshProUGUI textMeshPro;
    [SerializeField, ForceFill] private Image barImage;

    public void RefreshIcon(ItemSO itemSO, int num)
    {
        //Debug.Log(itemSO.ToString() + num);
        this.itemSO = itemSO;
        this.num = num;

        itemImage.sprite = itemSO.mainSprite;
        textMeshPro.text = num.ToString();
        
        // 调整bar的 颜色和长度
        if (barImage == null) return;
        barImage.color = itemSO.mainColor;
        if (num == 0)
        {
            barImage.fillAmount = 0f;
        }
        else
        {
            barImage.fillAmount = (float)num / itemSO.maxStack;
        }
        
    }
}
