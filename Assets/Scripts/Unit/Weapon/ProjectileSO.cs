using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/ProjectileSO")]
public class ProjectileSO : ScriptableObject 
{
    [Tooltip("子弹伤害")]
    public int damage;
    [Tooltip("子弹速度")]
    public float speed;
    [Tooltip("爆炸半径")]
    public float explosionRadius;
    [Tooltip("穿透次数")]
    public int penetration = 1;

    [ForceFill, Preview]
    public GameObject prefab;
    [HorizontalLine("只会与该layer的碰撞体进行检测")]
    public LayerMask targetLayer;
}
