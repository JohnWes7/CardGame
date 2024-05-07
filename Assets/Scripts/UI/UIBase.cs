using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class SingletonUIBase<T> : UIBase where T : UIBase
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("Canvas").GetComponentInChildren<T>(true);
                if (instance == null)
                {
                    Debug.LogError("Can't find " + typeof(T) + " in scene");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null || instance == this)
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
    public virtual void OpenUI() 
    {
        gameObject.SetActive(true);   
    }

    public virtual void CloseUI() 
    {
        gameObject.SetActive(false);
    }

    public abstract void Initialize(object args = null);
    public virtual void Destroy() 
    {
        Destroy(gameObject);
    }
}

