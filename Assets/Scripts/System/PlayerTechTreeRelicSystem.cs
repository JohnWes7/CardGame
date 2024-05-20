using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

/// <summary>
/// player 购买的时候会按照科技树遗物的解锁方式自动获得 新科技保存到遗物背包(通过监听购买事件)
/// </summary>
public class PlayerTechTreeRelicSystem : AbstractSystem
{
    /// <summary>
    /// 自动解锁科技树进度
    /// </summary>
    public class TechTreeUnlockProcess
    {

        public TechTreeNode techTreeNode;
        public bool active = false;

        public TechTreeUnlockProcess(TechTreeNode techTreeNode)
        {
            this.techTreeNode = techTreeNode;
        }

        public bool CheckUnlock(Dictionary<UnitSO, int> boughtDict)
        {
            // 检查是否能解锁
            return techTreeNode.CheckUnlock(boughtDict);
        }

        public bool CheckUnlock(Dictionary<UnitSO, int> boughtDict, out Dictionary<UnitSO, int[]> diff)
        {
            return techTreeNode.CheckUnlock(boughtDict, out diff);
        }

        public override string ToString()
        {
            return $"TechTreeUnlockProcess: {techTreeNode.name}\n";
        }
    }

    private Dictionary<TechTreeNode, TechTreeUnlockProcess> techTreeUnlockProcessDict = new ();
    private Dictionary<UnitSO, int> boughtDict = new();

    protected override void OnInit()
    {
        // 重置
        Reset();

        // 监听购买事件
        EventCenter.Instance.AddEventListener("BoughtUnit", OnBoughtUnitChange);
    }

    protected override void OnDeinit()
    {
        base.OnDeinit();
        // 取消事件
        EventCenter.Instance.RemoveEventListener("BoughtUnit", OnBoughtUnitChange);
    }

    public void Reset()
    {
        // 初始techTreeUnlockProcessList
        techTreeUnlockProcessDict ??= new();
        techTreeUnlockProcessDict.Clear();
        boughtDict ??= new();
        boughtDict.Clear();

        // 遍历所有遗物，找到科技树遗物 并加入list
        // 玩家拥有科技
        List<TechTreeNode> playerList = PlayerModel.Instance?.GetTechRelicInventory()?.GetTechList();

        this.GetModel<RelicModel>().allRelics.ForEach(relic =>
        {
            if (relic is TechTreeNode techTreeNode)
            {
                // 之后如果要解决重新加载的问题 一个是要保存bought (提取一个model到player中) 还有如果model中有已经解锁了的这边也得设置为unlock = true
                var techTreeUnlockProcess = new TechTreeUnlockProcess(techTreeNode);
                // 判断玩家是否已经解锁了这个科技 如果是有的话就设置为已激活 之后变不再检查是否需要 unlock
                if (playerList != null && playerList.Contains(techTreeNode))
                {
                    Debug.Log("player already unlock");
                    techTreeUnlockProcess.active = true;
                }
                techTreeUnlockProcessDict.Add(techTreeNode, techTreeUnlockProcess);
            }
        });

        Debug.Log($"PlayerTechTreeRelicSystem 遗物解锁系统初始化: OnInit techTreeUnlockProcessDict:\n {Johnwest.JWUniversalTool.DictToString(techTreeUnlockProcessDict)}");
    }

    public void OnBoughtUnitChange(object sender, object e)
    {
        Debug.Log("PlayerTechTreeRelicSystem 接收事件 BoughtUnitChange: \n 判断解锁新的炮塔");

        if (e is SpaceportShopProductInfo spaceportShopProductInfo)
        {
            // 把购买的单位加入购买字典 (相当于留一个收据)
            if (boughtDict.ContainsKey(spaceportShopProductInfo.unitSO))
            {
                boughtDict[spaceportShopProductInfo.unitSO]++;
            }
            else
            {
                boughtDict.Add(spaceportShopProductInfo.unitSO, 1);
            }

            // 判断能否解锁
            foreach (var item in techTreeUnlockProcessDict)
            {
                if (!item.Value.active)
                {
                    bool unlock = item.Value.CheckUnlock(boughtDict);
                    if (unlock)
                    {
                        Debug.Log($"PlayerTechTreeRelicSystem 解锁了新的炮塔: {item.Value.techTreeNode.unlockUnit.name}");

                        // 解锁成功 添加到遗物背包 设置为已激活
                        item.Value.active = true;
                        PlayerModel.Instance.GetTechRelicInventory().AddTech(item.Value.techTreeNode);
                    }

                }
            }

            #region debug log
            // debug 打印
            Debug.Log($"PlayerTechTreeRelicSystem: bought dict:\n{Johnwest.JWUniversalTool.DictToString(boughtDict)}\n");
            string debugStr = "";
            foreach (var item in techTreeUnlockProcessDict)
            {
                // 获取每个科技的解锁情况
                item.Value.techTreeNode.CheckUnlock(boughtDict, out Dictionary<UnitSO, int[]> outDict);
                debugStr += $"{item.Value.techTreeNode.name}:\n";
                foreach (KeyValuePair<UnitSO, int[]> pair in outDict)
                {
                    debugStr += $"{pair.Key.name} : {pair.Value[0]} / {pair.Value[1]}\n";
                }
            }
            Debug.Log($"全部的解锁项:\n {debugStr}");
            #endregion
        }
        else
        {
            Debug.LogWarning($"PlayerTechTreeRelicSystem 接收事件 BoughtUnitChang 参数传入错误: {e.GetType()}");
        }
    }
    

    public Dictionary<TechTreeNode, TechTreeUnlockProcess> GetTechTreeUnlockProcessDict()
    {
        return techTreeUnlockProcessDict;
    }

    public Dictionary<UnitSO, int> GetBoughtDict()
    {
        return boughtDict;
    }
}