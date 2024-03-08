using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomInspector;

public class BuildCostCurrency : MonoBehaviour
{
    [SerializeField, ForceFill] private TextMeshProUGUI currencyText;
    [SerializeField, ReadOnly] private UnitSO unitSO;

    public void RefreshCurrency(UnitSO unitSO)
    {
        // 显示unitSO 里面需要多少货币 以及玩家有多少货币
        this.unitSO = unitSO;
        currencyText.text = $"{unitSO.cost}/{PlayerModel.Instance.GetCurrency()}";
    }

    private void Update()
    {
        currencyText.text = $"{unitSO.cost}/{PlayerModel.Instance.GetCurrency()}";
    }
}
