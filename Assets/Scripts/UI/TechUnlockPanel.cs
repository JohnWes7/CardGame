using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using QFramework;
using TMPro;

/// <summary>
/// 通过command与PlayerTechTreeRelicSystem交互拿到所有的tech node
/// (以tech node为所有的key通过command拿到需要的所有数据) 
/// </summary>
public class TechUnlockPanel : UIBase, IController
{
    [SerializeField][ForceFill] private Transform context;
    [SerializeField][ReadOnly] private TechTreeNode rightShowTech;
    [SerializeField][AssetsOnly] private GameObject techPrefab;
    [SerializeField][ReadOnly] private List<GameObject> techList = new();

    [HorizontalLine("右侧显示")][SerializeField][ForceFill] private TextMeshProUGUI rightTestText;

    private void OnEnable()
    {
        UpdateTechList();
        UpdateRightShow();
    }

    /// <summary>
    /// 左侧显示所有科技节点
    /// </summary>
    public void UpdateTechList()
    {
        // 清空之前tech list 的所有元素
        foreach (GameObject item in techList) Destroy(item);

        techList.Clear();

        // 获得所有科技节点
        List<TechTreeNode> list = this.SendCommand(new GetTechSystemTechUnlockProcessListCommand());

        foreach (var item in list)
        {
            // 初始化节点显示
            GameObject go = Instantiate(techPrefab, context);
            if (!go.TryGetComponent(out TechUnlockIcon icon)) continue;

            icon.Init(item, this);
            techList.Add(go);
        }
    }

    /// <summary>
    /// 右侧显示当前 rightShowTech 的解锁进度
    /// 更改 rightShowTech 会自动调用这个方法
    /// </summary>
    public void UpdateRightShow()
    {
        // 先重置显示
        rightTestText.SetText("");
        if (rightTestText == null || rightShowTech == null) return;

        // 获取当前语言key
        string langKey = this.SendCommand(new GetLanguageStringKeyCommand());

        // 通过command 获得单个科技的解锁进度
        var unitNeedDict = this.SendCommand(new GetTechSystemProcessCommand(rightShowTech));
        string str = rightShowTech.unlockUnit.GetName(langKey) + "\n\n";
        foreach (var item in unitNeedDict)
        {
            str += $"{item.Key.GetName(langKey)} : {item.Value[0]}/{item.Value[1]}\n";
        }

        rightTestText.SetText(str);
    }

    public IArchitecture GetArchitecture()
    {
        return GameArchitecture.Interface;
    }

    public override void Initialize(object args = null)
    {
    }

    public void SetRightShowTech(TechTreeNode node)
    {
        rightShowTech = node;
        UpdateRightShow();
    }

}