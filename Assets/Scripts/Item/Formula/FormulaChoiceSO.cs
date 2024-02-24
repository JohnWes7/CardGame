using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/FormulaChoice")]
public class FormulaChoiceSO : ScriptableObject
{
    [SerializeField] public List<FormulaSO> formulas;

    public FormulaSO GetFormula(string name)
    {
        if (formulas == null)
        {
            return null;
        }

        foreach (FormulaSO item in formulas)
        {
            if (item.name == name)
            {
                return item;
            }
        }
        return null;
    } 
}
