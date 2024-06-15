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
        // 注册事件 当调用时 显示这个单位的信息并且在被destroy的时候注销事件
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

        // 必须先修改文字 因为影响之后修改大小 修改大小影响位置 所以修改位置放最后

        // 读取当前语言
        string langKey = this.GetUtility<LangUtility>().GetLanguageKey();

        // 设置全部显示
        nameText.text = args.unitObject.UnitSO.GetName(langKey);
        string desc = this.SendCommand(new GetProductDescCommand(args.unitObject.UnitSO, langKey));
        descText.SetText(desc);

        var mRectTransform = GetComponent<RectTransform>();

        // 设置大小
        mRectTransform.sizeDelta = new Vector2(mRectTransform.sizeDelta.x, descText.preferredHeight + 120f);

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

        //Debug.Log(uiPosition);
        mRectTransform.anchoredPosition = uiPosition;

        // 额外调整移动(避免出界看不到)

        // 获取屏幕边界
        float screenWidth = canvasRectTransform.sizeDelta.x;
        float screenHeight = canvasRectTransform.sizeDelta.y;

        // 判断如果上面部分超界那么就要往下移动
        // 获取UI边界
        Vector2 uiSize = mRectTransform.sizeDelta;
        Vector2 pivot = mRectTransform.pivot;

        // 检查是否超出屏幕边界并调整位置
        if (mRectTransform.anchoredPosition.x + (1 - pivot.x) * uiSize.x > screenWidth / 2)
        {
            uiPosition.x = screenWidth / 2 - (1 - pivot.x) * uiSize.x;
        }
        if (mRectTransform.anchoredPosition.x - pivot.x * uiSize.x < - screenWidth / 2)
        {
            uiPosition.x = -screenWidth / 2 + pivot.x * uiSize.x;
        }
        //Debug.Log($"if {mRectTransform.anchoredPosition.y + (1 - pivot.y) * uiSize.y} > {screenHeight / 2}");
        if (mRectTransform.anchoredPosition.y + (1 - pivot.y) * uiSize.y > screenHeight / 2)
        {
            uiPosition.y = screenHeight / 2 - (1 - pivot.y) * uiSize.y;
        }
        if (mRectTransform.anchoredPosition.y - pivot.y * uiSize.y < - screenHeight / 2)
        {
            uiPosition.y = -screenHeight / 2 + pivot.y * uiSize.y;
        }

        // 更新UI位置
        //Debug.Log($"pivot:{pivot}\no: {mRectTransform.anchoredPosition}, {uiPosition}");
        mRectTransform.anchoredPosition = uiPosition;
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
