using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomInspector;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;

/// <summary>
/// spaceport界面中的启动引擎按钮
/// 点击一次后会 改变按钮的文本内容 为确认 并且rectangle的颜色变为绿色
/// 再次点击后会触发一个异步加载场景的命令
/// </summary>
public class StartEngineButton : MonoBehaviour, IController, IPointerExitHandler
{

    [SerializeField, ForceFill]
    private TextMeshProUGUI text;
    [SerializeField, ForceFill]
    private Image changeColorImage;
    [SerializeField, ColorUsage(true, true)]
    private Color confirmColor;
    [SerializeField, ColorUsage(true, true)]
    private Color originColor;
    [SerializeField, SelfFill]
    private Button button;
    
    public bool isConfirm = false;

    private void Awake()
    {
        button.onClick.AddListener(Button_OnClick);
    }

    public void Button_OnClick()
    {
        if (isConfirm)
        {
            // 触发ship保存memento事件
            this.SendCommand(new TriggerSaveShipMementoCommand());
            // 保存数据命令
            this.SendCommand(new SaveDataCommand());

            // 跳转场景命令
            var command = new AsyncLoadSceneCommand("SampleScene", this);
            this.SendCommand(command);
        }
        else
        {
            // 获取当前关卡index
            var command = new GetStageIndexCommand();
            this.SendCommand(command);

            changeColorImage.color = confirmColor;
            text.SetText($"目的地: #{command.mStageIndex:D2}");
            isConfirm = true;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        changeColorImage.color = originColor;
        text.SetText("发动引擎");
        isConfirm = false;
    }
}
