using System;
using System.Collections.Generic;
using CustomInspector;
using DG.Tweening;
using QFramework;
using UnityEngine;

[RequireComponent(typeof(UIBase))]
public class CloseUIMoveAni : MonoBehaviour, IController
{
    public Vector3 targetPos = new(1920f, 0);
    public float duration = 0.8f;
    public Ease ease = Ease.OutExpo;
    [SelfFill] public UIBase uiBase;

    public void CloseUI()
    {
        // 删除已有动画进程
        uiBase.transform.DOKill(false);

        // 执行动画命令
        var command = new UIMoveCommand(uiBase.transform, uiBase.transform.position, targetPos)
        {
            action = () => uiBase.CloseUI(),
            ease = ease,
            duration = duration
        };
        this.SendCommand(command);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}