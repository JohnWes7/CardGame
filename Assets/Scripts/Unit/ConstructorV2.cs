using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

public class ConstructorV2 : UnitObject, IShipUnit, IBeClick, IExtraUnitObjectData
{
    [SerializeField, ReadOnly] protected MonoInterface<IShipController> ship;

    [SerializeField, ForceFill]
    private FormulaChoiceSO allFormula;
    [SerializeField, ReadOnly]
    private FormulaSO curFormula;

    [SerializeField, ReadOnly]
    private float timer;

    public void SetShip(IShipController sc)
    {
        ship.InterfaceObj = sc;
    }

    public IShipController GetShip()
    {
        return ship.InterfaceObj;
    }

    public Dictionary<string, object> GetExtraData()
    {
        // 如果当前没有配方 则直接返回null
        if (curFormula == null)
        {
            return null;
        }

        // 如果有配方保存配方名字
        Dictionary<string, object> result = new Dictionary<string, object>();
        result.Add("formulaName", curFormula.name);
        return result;
    }

    public void SetExtraData(Dictionary<string, object> data)
    {
        Debug.Log("Construct:SetExtraData: data : " + Johnwest.JWUniversalTool.DictToString(data));
        if (data is null)
        {
            return;
        }

        if (data.TryGetValue("formulaName", out object name))
        {
            if (name is string @string)
            {
                Debug.Log($"更改配方: {allFormula.GetFormula(@string)}");
                ChangeFormula(@string);
            }
            else
            {
                Debug.LogError("Construct:SetExtraData: can not get formula name: " + Johnwest.JWUniversalTool.DictToString(data));
            }
        }
    }

    /// <summary>
    /// 更改制造机制造的 物品
    /// </summary>
    /// <param name="index"></param>
    public void ChangeFormula(int index)
    {
        // 如果是越界说明change到null
        if (index < 0 || index >= allFormula.formulas.Count)
        {
            curFormula = null;
            return;
        }

        // 如果换了还是相同的就什么也不干
        if (curFormula == allFormula.formulas[index])
        {
            return;
        }

        // 如果配方改动则清除之前里面的东西 (后续可能要缓存成把还有的物品收回背包)
        curFormula = allFormula.formulas[index];
    }

    public void ChangeFormula(string name)
    {
        curFormula = allFormula.GetFormula(name);
    }

    public void BeClick(object sender)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (timer < curFormula.timeNeed_Second)
        {
            timer += Time.deltaTime;
        }
    }
}
