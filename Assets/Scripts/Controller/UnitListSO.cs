using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "ScriptableObject/UnitListSO")]
public class UnitListSO : ScriptableObject
{
    [Foldout]
    public List<UnitSO> unitSOList;
}
