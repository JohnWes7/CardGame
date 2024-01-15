using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Formula")]
public class FormulaSO : ScriptableObject
{
    [System.Serializable]
    public struct ItemSOValue
    {
        public ItemSO item;
        public int value;
    }

    [SerializeField] public List<ItemSOValue> rawMat;
    [SerializeField] public List<ItemSOValue> outPut;
    [SerializeField] public float timeNeed_Second;

    public override string ToString()
    {
        List<string> raw = new List<string>();
        foreach (var item in rawMat)
        {
            raw.Add($"{item.value} {item.item.name}");
        }

        List<string> output = new List<string>();
        foreach (var item in outPut)
        {
            output.Add($"{item.value} {item.item.name}");
        }

        return string.Join("+", raw) + " --> " + string.Join("+", output);
    }
}
