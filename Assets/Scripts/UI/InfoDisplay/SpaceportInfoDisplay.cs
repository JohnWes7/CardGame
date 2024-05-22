using CustomInspector;
using System;
using System.Collections.Generic;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceportInfoDisplay : MonoBehaviour, IController
{
    [ForceFill]
    public TextMeshProUGUI nameText;
    [ForceFill]
    public TextMeshProUGUI descText;
    [ReadOnly]
    public Canvas canvas;

    private void Awake()
    {
        // 注册事件 当调用时 显示这个单位的信息并且
        this.RegisterEvent<SendShowUnitDetailEventCommand.ShowUnitDetailEvent>(UpdateInfo)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        // 设置canvas
        canvas = GetComponentInParent<Canvas>();
        // 关闭自身
        gameObject.SetActive(false);
    }

    public void UpdateInfo(SendShowUnitDetailEventCommand.ShowUnitDetailEvent args)
    {
        // 如果传入的是空说明关闭
        if (args.unitObject == null)
        { 
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        // 设置位置
        Vector3 worldPosition = args.unitObject.transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        Vector2 uiPosition;

        // 处理不同的Canvas Render Mode
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null, out uiPosition);
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, canvas.worldCamera, out uiPosition);
        }
        else
        {
            Debug.LogError("Unsupported Render Mode");
            return;
        }

        Debug.Log(uiPosition);
        var mRectTransform = GetComponent<RectTransform>();
        mRectTransform.anchoredPosition = uiPosition;

        
        // 读取当前语言
        string langKey = this.GetUtility<LangUtility>().GetLanguageKey();

        // 设置全部显示
        nameText.text = args.unitObject.UnitSO.GetName(langKey);
        string desc = this.SendCommand(new GetProductDescCommand(args.unitObject.UnitSO, langKey));
        descText.SetText(desc);

        // 设置大小
        mRectTransform.sizeDelta = new Vector2(mRectTransform.sizeDelta.x, descText.preferredHeight + 120f);
    }

    public void UpdateRange(SendShowUnitDetailEventCommand.ShowUnitDetailEvent args)
    {
        
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
