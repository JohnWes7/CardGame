using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

/// <summary>
/// player 购买的时候会按照科技树遗物的解锁方式自动获得 新科技保存到遗物背包
/// </summary>
public class PlayerTechTreeRelicSystem : AbstractSystem
{
    /// <summary>
    /// 自动解锁科技树遗物加入到商店池子中
    /// </summary>
    public class TechTreeUnlockProcess
    {

        public TechTreeNode techTreeNode;
        public bool isUnlock = false;
        public bool active = false;

        public TechTreeUnlockProcess(TechTreeNode techTreeNode)
        {
            this.techTreeNode = techTreeNode;
        }

        public bool CheckUnlock(Dictionary<UnitSO, int> boughtDict)
        {
            isUnlock = techTreeNode.CheckUnlock(boughtDict);
            return isUnlock;
        }


        public override string ToString()
        {
            return $"TechTreeUnlockProcess: {techTreeNode.name}\n";
        }
    }

    private List<TechTreeUnlockProcess> techTreeUnlockProcessList = new List<TechTreeUnlockProcess>();
    private Dictionary<UnitSO, int> boughtDict = new Dictionary<UnitSO, int>();

    protected override void OnInit()
    {
        // 初始techTreeUnlockProcessList
        techTreeUnlockProcessList ??= new();
        techTreeUnlockProcessList.Clear();
        boughtDict ??= new();
        boughtDict.Clear();

        // 遍历所有遗物，找到科技树遗物 并加入list
        this.GetModel<RelicModel>().allRelics.ForEach(relic =>
        {
            if (relic is TechTreeNode techTreeNode)
            {
                // 之后如果要解决重新加载的问题 一个是要保存bought (提取一个model到player中) 还有如果model中有已经解锁了的这边也得设置为unlock = true
                var techTreeUnlockProcess = new TechTreeUnlockProcess(techTreeNode);
                // 判断玩家是否已经解锁了这个科技
                if (PlayerModel.Instance.GetTechRelicInventory().GetTechList().Contains(techTreeNode))
                {
                    techTreeUnlockProcess.isUnlock = true;
                }
                techTreeUnlockProcessList.Add(techTreeUnlockProcess);
            }
        });

        

        Debug.Log($"PlayerTechTreeRelicSystem 遗物解锁系统初始化: OnInit techTreeUnlockProcessList:\n {Johnwest.JWUniversalTool.ListToString(techTreeUnlockProcessList)}");

        EventCenter.Instance.AddEventListener("BoughtUnit", OnBoughtUnitChange);
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
            foreach (TechTreeUnlockProcess item in techTreeUnlockProcessList)
            {
                if (!item.isUnlock)
                {
                    item.CheckUnlock(boughtDict);
                    if (item.isUnlock)
                    {
                        Debug.Log($"PlayerTechTreeRelicSystem 解锁了新的炮塔: {item.techTreeNode.unlockUnit.name}");

                        // 解锁成功 添加到遗物背包
                        item.active = true;
                        PlayerModel.Instance.GetTechRelicInventory().AddTech(item.techTreeNode);
                    }

                }
            }

            #region debug log
            // debug 打印
            Debug.Log($"PlayerTechTreeRelicSystem: boughtdict:\n{Johnwest.JWUniversalTool.DictToString(boughtDict)}\n");
            string debugStr = "";
            foreach (var item in techTreeUnlockProcessList)
            {
                // 获取每个科技的解锁情况
                Dictionary<UnitSO, int[]> outDict = new Dictionary<UnitSO, int[]>();
                item.techTreeNode.CheckUnlock(boughtDict, out outDict);
                debugStr += $"{item.techTreeNode.name}:\n";
                foreach (var pair in outDict)
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
}