using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using CustomInspector;
using UnityEngine.EventSystems;

public class UnitIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image mainImg;
    [SerializeField] private UnitSO unitSO;
    [SerializeField, SelfFill] private Button button;

    // 鼠标悬停的时候显示二级ui
    [SerializeField, ForceFill] private GameObject secondUI;
    [SerializeField, ForceFill] private TextMeshProUGUI secondUIUnitName;

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

        // 设置二级ui的名字
        secondUIUnitName.text = unitSO.name;
    }

    public void ClickDebug()
    {
        Debug.Log("unit select");
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        //鼠标进入的时候显示二级ui
        secondUI.SetActive(true);
        // 并且更改将canvas作为父物体
        secondUI.transform.SetParent(GameObject.Find("Canvas").transform);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开的时候隐藏二级ui
        secondUI.SetActive(false);
        secondUI.transform.SetParent(transform);
    }
}
