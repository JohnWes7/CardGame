using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/FormulaChoice")]
public class FormulaChoiceSO : ScriptableObject
{
    [SerializeField] public List<FormulaSO> formulas;
}
