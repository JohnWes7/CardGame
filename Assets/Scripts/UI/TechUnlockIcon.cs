using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TechUnlockIcon : MonoBehaviour, IPointerEnterHandler, IController
{
    [SerializeField, ForceFill]
    private TextMeshProUGUI techName;
    [SerializeField, ReadOnly]
    private TechTreeNode node;
    [SerializeField, ReadOnly]
    private TechUnlockPanel techPanel;

    public void Init(TechTreeNode node, TechUnlockPanel techPanel)
    {
        // 初始化icon
        this.techPanel = techPanel;
        this.node = node;

        // 设置icon的显示
        // 获取名字
        string unlockName = node.unlockUnit.GetName(this.GetUtility<LangUtility>().GetLanguageKey());
        //Debug.Log(unlockName + " " + this.GetUtility<LangUtility>().GetLanguageKey());
        // 如果是已经解锁了的显示绿色 如果没有就还是显示白色
        techName.SetText(this.SendCommand(new IsNodeUnlockCommand(node))
            ? $"<color=green>{unlockName}</color>"
            : unlockName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (techPanel == null)
        {
            return;
        }

        // 设置panel 要显示的科技为这个icon 的科技
        techPanel.SetRightShowTech(node);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }
}
