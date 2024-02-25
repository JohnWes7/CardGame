using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/ProjectileSO")]
public class ProjectileSO : ScriptableObject 
{
    public int damage;
    public float speed;
    public float explosionRange;
    public float shieldCoefficient;
    public float armourCoefficient;
    public float bodyCoefficient;
    public GameObject prefab;
    public List<string> targetTag;
    public LayerMask targetLayer;
}
