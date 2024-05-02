using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unlock Relic", menuName = "Relic/UnlockRelic")]
public class TechTreeNode : RelicSO
{
    public UnitSO unlockUnit;
    public List<UnitNumPair> requireUnit;

    public override void OnReceive()
    {
        Debug.Log("解锁了" + unlockUnit.name);
    }

    public bool CheckUnlock(Dictionary<UnitSO, int> boughtDict)
    {
        // 检查购买的玩家总共购买的物品是否满足解锁遗物的条件
        bool condition = true;
        foreach (var pair in requireUnit)
        {
            // 判断每一个行的物品是否满足需求
            if (boughtDict.GetValueOrDefault(pair.unitSO, 0) < pair.num)
            {
                condition = false;
                break;
            }

        }

        return condition;
    }

    /// <summary>
    /// 返回出当前需求值和拥有值
    /// </summary>
    /// <param name="boughtDict"></param>
    /// <param name="diff"></param>
    /// <returns></returns>
    public bool CheckUnlock(Dictionary<UnitSO, int> boughtDict, out Dictionary<UnitSO, int[]> diff)
    {
        // 检查购买的玩家总共购买的物品是否满足解锁遗物的条件
        diff = new Dictionary<UnitSO, int[]>();
        foreach (UnitNumPair pair in requireUnit)
        {
            diff[pair.unitSO] = new int[2];
            // 需求值和拥有值
            diff[pair.unitSO][0] = pair.num;
            diff[pair.unitSO][1] = boughtDict.GetValueOrDefault(pair.unitSO, 0);
        }

        return CheckUnlock(boughtDict);
    }
}
