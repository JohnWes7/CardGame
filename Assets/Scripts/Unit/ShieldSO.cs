using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName ="ScriptableObject/ShieldSO")]
public class ShieldSO : ScriptableObject
{
    [System.Serializable]
    public class ItemRechargeInfo
    {
        public ItemSO itemSO;
        public int rechargeNum;
    }

    [HorizontalLine("护盾运行属性")]
    public float shieldRadius = 5;
    public int shieldCapacity = 100;
    public float rechargeGap = 1;
    [HorizontalLine("默认每次护盾回复量")]
    public int defaultRechargeNum = 5;
    public List<ItemRechargeInfo> itemRechargeInfo; 
    [HorizontalLine("护盾重启速度")]
    public float restartTime = 5;
}
