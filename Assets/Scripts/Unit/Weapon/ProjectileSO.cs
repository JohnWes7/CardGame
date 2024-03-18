using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/ProjectileSO")]
public class ProjectileSO : ScriptableObject 
{
    public int damage;
    public float speed;
    public float explosionRadius;
    public int penetration = 1;

    [ForceFill, Preview]
    public GameObject prefab;
    [HorizontalLine("只会与该layer的碰撞体进行检测")]
    public LayerMask targetLayer;
}
