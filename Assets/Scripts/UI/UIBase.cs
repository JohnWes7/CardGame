using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SingletonUIBase<T> : UIBase where T : UIBase
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public override void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public override void Initialize(object args = null)
    {
        
    }

    public override void Destroy()
    {
        Destroy(gameObject);
    }
}

public abstract class UIBase : MonoBehaviour
{
    public abstract void OpenUI();
    public abstract void CloseUI();
    public abstract void Initialize(object args = null);
    public abstract void Destroy();
}

