using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonoInterface<T>
{
    [SerializeField] private T interfaceObj;
#pragma warning disable IDE0052 // 删除未读的私有成员
    [SerializeField] private MonoBehaviour monoBehaviour;
#pragma warning restore IDE0052 // 删除未读的私有成员

    public MonoInterface(T itf)
    {
        interfaceObj = itf;
        if (itf is MonoBehaviour)
        {
            monoBehaviour = itf as MonoBehaviour;
        }
        else
        {
            monoBehaviour = null;
        }
    }

    public T InterfaceObj
    {
        get { return interfaceObj; }

        set
        {
            interfaceObj = value;
            if (value is MonoBehaviour)
            {
                monoBehaviour = value as MonoBehaviour;
            }
            else
            {
                monoBehaviour = null;
            }
        }
    }

    public MonoBehaviour MonoBehaviour { get => monoBehaviour; }
}
