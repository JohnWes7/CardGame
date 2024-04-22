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
        bool condition = false;
        foreach (var pair in requireUnit)
        {
            // 判断每一个行的物品是否满足需求
            if (boughtDict.GetValueOrDefault(pair.unitSO, 0) >= pair.num)
            {
                condition = true;
            }
            else
            {
                condition = false;
                break;
            }
        }

        return condition;
        
    }
}