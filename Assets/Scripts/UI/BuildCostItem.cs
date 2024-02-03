using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEngine.UI;
using TMPro;

public class BuildCostItem : MonoBehaviour
{
    [SerializeField, ForceFill] private Image itemSprite;
    [SerializeField, ForceFill] private TextMeshProUGUI text;
    [SerializeField, ReadOnly] private UnitSO.ItemCost itemCost;
    private const string FREE_BUILD_TEXT = "Free to Build";
    

    private void OnEnable()
    {
        PlayerInventory.Instance.OnInventoryChange += Instance_OnInventoryChange;
    }

    private void OnDisable()
    {
        PlayerInventory.Instance.OnInventoryChange -= Instance_OnInventoryChange;
    }

    private void Instance_OnInventoryChange(object sender, PlayerInventory.InventoryEventArgs e)
    {
        Refresh(itemCost);
    }

    public void Refresh(UnitSO.ItemCost itemCost)
    {
        this.itemCost = itemCost;
        if (itemCost == null)
        {
            text.text = FREE_BUILD_TEXT;
            return;
        }

        itemSprite.sprite = itemCost.itemSO.mainSprite;

        int inventoryNum = PlayerInventory.Instance.GetItemNum(itemCost.itemSO);
        int cost = itemCost.cost;

        string textShow = $"{inventoryNum}/{cost}";
        text.text = textShow;
    }
}
