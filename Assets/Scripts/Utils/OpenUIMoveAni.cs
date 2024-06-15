using System;
using System.Collections.Generic;
using CustomInspector;
using DG.Tweening;
using QFramework;
using UnityEngine;

[RequireComponent(typeof(UIBase))]
public class OpenUIMoveAni : MonoBehaviour, IController
{
    public Vector3 startPos = new Vector3(1920f, 0f);
    public Vector3 targetPos;
    public float duration = 0.8f;
    public Ease ease = Ease.OutExpo;
    [SelfFill]
    public UIBase uiBase;


    public void OpenUI()
    {
        // 删除已有动画进程
        //Debug.Log(uiBase.transform.DOKill(false));
        uiBase.transform.DOKill(false);

        // 打开面板
        uiBase.OpenUI();
        // 执行命令
        var command = new UIMoveCommand(uiBase.transform, startPos, targetPos)
        {
            duration = duration,
            ease = ease
        };
        this.SendCommand(command);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
