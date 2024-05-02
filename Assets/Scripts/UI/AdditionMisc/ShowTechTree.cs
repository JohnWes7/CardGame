using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 显示一个窗口 显示当前unitso所可以解锁的科技树
/// </summary>
public class ShowTechTree : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject displayPanelPrefabs;
    public UIBase displayePanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayePanel == null)
        {
            displayePanel = Instantiate(displayPanelPrefabs, transform).GetComponent<UIBase>();
        }
        displayePanel.OpenUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayePanel != null)
        {
            displayePanel.CloseUI();
        }
    }
}
