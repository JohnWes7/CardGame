using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/UnitListSO")]
public class UnitListSO : ScriptableObject
{
    public List<UnitSO> unitSOList;
}
