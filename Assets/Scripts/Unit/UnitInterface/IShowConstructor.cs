using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IShowConstructor
{
    public void Init(FormulaChoiceSO allFormula, FormulaSO curFormula);
    public void AddChangeValueListener(UnityAction<int> action);
    public void RemoveChangeValueListener(UnityAction<int> action);
    public void AddCloseListener();

    // 暂时是打印string表示现在状态
    public void SetDataObserver(Func<string> dataToStringFunc);

    public GameObject GetGameObject();
}
