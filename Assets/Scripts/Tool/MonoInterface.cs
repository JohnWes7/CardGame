using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonoInterface<T>
{
    [SerializeField] private T interfaceObj;
    [SerializeField] private MonoBehaviour monoBehaviour;

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
}
