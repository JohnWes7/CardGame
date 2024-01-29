using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RightBarInventoryItemIcon : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private int num;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void RefreshIcon(ItemSO itemSO, int num)
    {
        //Debug.Log(itemSO.ToString() + num);
        this.itemSO = itemSO;
        this.num = num;

        itemImage.sprite = itemSO.mainSprite;
        textMeshPro.text = num.ToString();
    }
}
