using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

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

    /// <summary>
    /// 子弹在有弹药的时候 info
    /// </summary>
    public List<MagazineInfo> magazineInfos;
    [Foldout]
    public ProjectileSO defaultProjectile;

    // 子弹消耗速率
    [ShowMethod(nameof(GetAmmoConsumeRate))]
    
    // 炮塔属性
    public float fireGap;   // 发射间隔
    public float radius;    // 射程半径
    public float rotateSpeed;   // 旋转速度

    // 获得弹药消耗速率
    public float GetAmmoConsumeRate()
    {
        try
        {
            // 消耗速率计算 弹夹子弹数量/每秒发射量 = 弹药item消耗量/s
            float rate = (1 / fireGap) / magazineInfos[0].projectileInOneMagazineNum;
            return rate;
        }
        catch (System.Exception)
        {
            //Debug.LogError("弹药消耗速率计算错误");
        }
        
        return -1f;
    }
}

public interface IShopDescription
{
    string GetDescription(string langKey);
}