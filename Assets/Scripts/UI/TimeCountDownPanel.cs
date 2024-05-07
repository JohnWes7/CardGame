using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CustomInspector;
using QFramework;

public class TimeCountDownPanel : SingletonUIBase<TimeCountDownPanel>, IController
{
    [SerializeField, ForceFill] private TextMeshProUGUI timeText;


    protected override void Awake()
    {
        base.Awake();
        // 注册事件
        EventCenter.Instance.AddEventListener("BattleTimeCountDown", TimeCountDown);
    }

    private void OnDestroy()
    {
        // 移除事件
        EventCenter.Instance.RemoveEventListener("BattleTimeCountDown", TimeCountDown);
    }

    private void OnEnable()
    {
        // 更新时间
        UpdateTimeText(0);
    }

    /// <summary>
    /// BattleTimeCountDown 事件回调
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void TimeCountDown(object sender, object args)
    {
        if (args is float floatArgs)
        {
            UpdateTimeText(floatArgs);
        }
    }

    /// <summary>
    /// 更新时间显示
    /// </summary>
    /// <param name="time"></param>
    public void UpdateTimeText(float time)
    {
        timeText.text = time.ToString("F2");
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}