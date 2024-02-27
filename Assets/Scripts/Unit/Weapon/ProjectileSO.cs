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
    [HorizontalLine("应该现在全是扣血就可以了")]
    public float shieldCoefficient;
    public float armourCoefficient;
    public float bodyCoefficient;
    [ForceFill, Preview]
    public GameObject prefab;
    [HorizontalLine("只会与该layer的碰撞体进行检测")]
    public LayerMask targetLayer;
}
