using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class RightBarInventoryCurrencyItem : MonoBehaviour
{
    [SerializeField, AssetsOnly, ForceFill] private ItemSO item;
    [SerializeField] private TMPro.TextMeshProUGUI countText;

    private void Update()
    {
        countText.text = PlayerModel.Instance.GetInventory().GetItemNum(item).ToString();
    }
}
