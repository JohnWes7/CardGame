using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class ConstructorClickPanel : MonoBehaviour, IShowConstructor
{   
    // 之后可能需要改成单例 就像设置窗口只能打开一个
    // 选择配方
    [SerializeField] private TMP_Dropdown dropdown;

    // 显示当前制造机里面有配方的东西多少
    [SerializeField] private object data;
    private Func<string> dataToStringFunc;
    [SerializeField] private TMP_Text formulaText;

    public void AddChangeValueListener(UnityAction<int> action)
    {
        dropdown.onValueChanged.AddListener(action);
    }

    public void RemoveChangeValueListener(UnityAction<int> action)
    {
        dropdown.onValueChanged.RemoveListener(action);
    }

    public void AddCloseListener()
    {
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Init(FormulaChoiceSO allFormula, FormulaSO curFormula)
    {

        // 清空 observe 和 option
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.ClearOptions();
        gameObject.SetActive(true);

        int cur = allFormula.formulas.Count;

        // 显示所有选项并且得到当前的配方是什么
        List<string> options = new List<string>();
        for (int i = 0; i < allFormula.formulas.Count; i++)
        {
            options.Add(allFormula.formulas[i].ToString());
            if (allFormula.formulas[i] == curFormula)
            {
                cur = i;
            }
        }
        options.Add("Stop");

        dropdown.AddOptions(options);

        dropdown.value = cur;
    }

    public void UpdateFormulaText()
    {
        //if (rawMatNum == null || rawMatNum.Count == 0)
        //{
        //    formulaText.text = "";
        //    return;
        //}

        //if (outPutNum == null || outPutNum.Count == 0)
        //{
        //    formulaText.text = "";
        //    return;
        //}

        //List<string> raw = new List<string>();
        //foreach (var item in rawMatNum)
        //{
        //    raw.Add($"{item.Key.item.name}:{item.Value}");
        //}

        //List<string> output = new List<string>();
        //foreach (var item in outPutNum)
        //{
        //    output.Add($"{item.Key.item.name}:{item.Value}");
        //}

        //formulaText.text = string.Join("+", raw) + "-->" + string.Join("+", output);

        formulaText.text = dataToStringFunc();
    }

    private void Update()
    {
        UpdateFormulaText();
    }

    public void SetDataObserver(Func<string> dataToStringFunc)
    {
        this.dataToStringFunc = dataToStringFunc;
    }

    public GameObject GetGameObject()
    {
        return gameObject == null ? null : gameObject;
    }
}
