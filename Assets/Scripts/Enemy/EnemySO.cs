using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "ScriptableObject/EnemySO")]
public class EnemySO : ScriptableObject
{
    public readonly int maxHP;
    public float speed;
    public GameObject prefab;
    public float warningRange;
    public LayerMask warningLayer;
}
