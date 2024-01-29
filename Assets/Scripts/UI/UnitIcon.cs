using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitIcon : MonoBehaviour
{
    [SerializeField] private Image mainImg;
    [SerializeField] private UnitSO unitSO;
    [SerializeField] private Button button;

    public Button.ButtonClickedEvent OnClick
    {
        get
        {
            return button.onClick;
        }
    }

    public UnitSO UnitSO { get => unitSO; set => unitSO = value; }

    public void RefreshIcon(UnitSO unitSO)
    {
        this.unitSO = unitSO;
        mainImg.sprite = unitSO.fullsizeSprite;
    }

    public void ClickDebug()
    {
        Debug.Log("unit select");
    }
}
