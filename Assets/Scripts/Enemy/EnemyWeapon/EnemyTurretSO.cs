using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyTurretSO")]
public class EnemyTurretSO : ScriptableObject
{
    [ForceFill, AssetsOnly]
    public ProjectileSO projectileSO;
    public float radius;
    public float fireGap;
    public float rotateSpeed;
    public LayerMask searchTargetLayer;
}
