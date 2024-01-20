using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/TurretSO")]
public class TurretSO : ScriptableObject
{
    [System.Serializable]
    public class MagazineInfo
    {
        public ItemSO magazineItem;
        public int projectileInOneMagazineNum;
        public ProjectileSO projectileSO;
    }

    public List<MagazineInfo> magazineInfos;
    public float fireGap;
    public float range;
    public float rotateSpeed;
}