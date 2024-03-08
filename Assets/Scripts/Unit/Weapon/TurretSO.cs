using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using UnityEditorInternal.Profiling.Memory.Experimental;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/TurretSO")]
public class TurretSO : ScriptableObject
{
    [System.Serializable]
    public class MagazineInfo
    {
        public ItemSO magazineItem;
        public int projectileInOneMagazineNum;
        [Preview, Foldout] public ProjectileSO projectileSO;
    }

    public List<MagazineInfo> magazineInfos;

    [ShowMethod(nameof(GetAmmoConsumRate))]
    public float fireGap;
    public float radius;
    public float rotateSpeed;

    // 获得弹药消耗速率
    public float GetAmmoConsumRate()
    {
        // 消耗速率计算 弹夹子弹数量/每秒发射量 = 弹药item消耗量/s
        float rate = (1 / fireGap) / magazineInfos[0].projectileInOneMagazineNum;
        return rate;
    }
}