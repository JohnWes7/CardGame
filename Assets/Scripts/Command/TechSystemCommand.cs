using UnityEngine;
using System.Collections.Generic;
using QFramework;

/// <summary>
/// 从relic model中 获取所有科技树节点
/// </summary>
public class GetTechSystemTechUnlockProcessListCommand : AbstractCommand<List<TechTreeNode>>
{
    protected override List<TechTreeNode> OnExecute()
    {
        // 从system中获得所有科技树节点
        var dict = this.GetSystem<PlayerTechTreeRelicSystem>().GetTechTreeUnlockProcessDict();

        // 转化为List<TechTreeNode>
        List<TechTreeNode> list = new List<TechTreeNode>();
        foreach (var item in dict)
        {
            list.Add(item.Key);
        }   
        return list;
    }
}

/// <summary>
/// 获得指定科技系统有没有解锁
/// </summary>
public class GetTechSystemProcessCommand : AbstractCommand<Dictionary<UnitSO, int[]>>
{
    public TechTreeNode node;

    public GetTechSystemProcessCommand(TechTreeNode node)
    {
        this.node = node;
    }


    protected override Dictionary<UnitSO, int[]> OnExecute()
    {
        if (node == null)
        {
            Debug.LogWarning("GetTechSystemProcessCommand: node is null");
            return null;
        }

        // 获得所有购买
        PlayerTechTreeRelicSystem system = this.GetSystem<PlayerTechTreeRelicSystem>();
        var boughtDict = system.GetBoughtDict();
        
        // 检查进度
        node.CheckUnlock(boughtDict, out Dictionary<UnitSO, int[]> diff);
        return diff;
    }
}

public class IsNodeUnlockCommand : AbstractCommand<bool>
{
    public TechTreeNode node;

    public IsNodeUnlockCommand(TechTreeNode node)
    {
        this.node = node;
    }

    protected override bool OnExecute()
    {
        // 从system中获得所有科技树节点
        var dict = this.GetSystem<PlayerTechTreeRelicSystem>().GetTechTreeUnlockProcessDict();
        if (dict.TryGetValue(node, out var process))
        {
            return process.active;
        }

        // 没有找到node 对应的解锁
        Debug.LogError("IsNodeUnlockCommand: 没有该 tech node");
        return false;
    }
}
