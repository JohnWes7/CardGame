using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomInspector;

public class CurrencyPanel : MonoBehaviour
{
    [SerializeField, ForceFill] private TextMeshProUGUI currencyText;

    private void Update()
    {
        currencyText.text = PlayerModel.Instance.GetCurrency().ToString();
    }
}
