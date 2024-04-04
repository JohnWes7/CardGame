using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unlock Relic", menuName = "Relic/UnlockRelic")]
public class TechTreeNode : Relic
{
    public UnitSO unlockUnit;
    public List<UnitNumPair> requireUnit;

    public override void OnReceive()
    {
        Debug.Log("解锁了" + unlockUnit.name);
    }
}